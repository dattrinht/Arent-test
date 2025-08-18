namespace HealthApp.Infrastructure.Repositories;

internal class ColumnRepository : IColumnRepository
{
    private readonly HealthAppContext _dbContext;

    public ColumnRepository(HealthAppContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Column> CreateAsync(ColumnDetailDto dto, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        var entity = new Column
        {
            Id = IdGenHelper.CreateId(),
            Slug = dto.Slug.Trim().ToLower(),
            Title = dto.Title.Trim(),
            Summary = dto.Summary,
            Content = dto.Content,
            DisplayImage = string.IsNullOrWhiteSpace(dto.DisplayImage) ? null : dto.DisplayImage.Trim(),
            IsPublished = dto.IsPublished,
            PublishedAt = dto.PublishedAt,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        _dbContext.Columns.Add(entity);

        var requestTaxonomyIds = dto.Taxonomies?.Select(x => x.Id) ?? [];
        var validTaxonomyIds = await _dbContext.ColumnTaxonomies
            .Where(t => requestTaxonomyIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync(ct);

        var associations = validTaxonomyIds
            .ConvertAll(taxonomyId => new ColumnTaxonomyAssociation
            {
                ProfileId = entity.ProfileId,
                ColumnId = entity.Id,
                TaxonomyId = taxonomyId,
                CreatedAt = now,
            });

        _dbContext.ColumnTaxonomyAssociations.AddRange(associations);
        await _dbContext.SaveChangesAsync(ct);

        entity.ColumnTaxonomies = associations;
        return entity;
    }

    public async Task<Column?> UpdateAsync(ColumnDetailDto dto, CancellationToken ct = default)
    {
        var entity = await _dbContext.Columns.FirstOrDefaultAsync(c => c.Id == dto.Id, ct);
        if (entity is null) return null;

        var now = DateTime.UtcNow;

        entity.Title = dto.Title;
        entity.Slug = dto.Slug.ToLower();
        entity.Summary = dto.Summary;
        entity.Content = dto.Content;
        entity.DisplayImage = dto.DisplayImage;
        entity.IsPublished = dto.IsPublished;
        entity.PublishedAt = dto.IsPublished ? now : entity.PublishedAt;
        entity.UpdatedAt = now;

        var requestTaxonomyIds = dto.Taxonomies?.Select(x => x.Id) ?? [];
        var validTaxonomyIds = await _dbContext.ColumnTaxonomies
            .Where(t => requestTaxonomyIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync(ct);

        var currentTaxonomyIds = await _dbContext.ColumnTaxonomyAssociations
            .Where(a => a.ColumnId == entity.Id)
            .Select(a => a.TaxonomyId)
            .ToArrayAsync(ct);

        var toAdd = validTaxonomyIds.Except(currentTaxonomyIds).ToList();
        var toRemove = currentTaxonomyIds.Except(validTaxonomyIds).ToList();

        if (toAdd.Count > 0)
        {
            var newAssociations = toAdd.ConvertAll(taxonomyId => new ColumnTaxonomyAssociation
            {
                Id = IdGenHelper.CreateId(),
                ColumnId = entity.Id,
                TaxonomyId = taxonomyId,
                CreatedAt = now,
            });

            _dbContext.ColumnTaxonomyAssociations.AddRange(newAssociations);
            entity.ColumnTaxonomies = newAssociations;
        }

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            if (toRemove.Count > 0)
            {
                await _dbContext.ColumnTaxonomyAssociations
                    .Where(a => a.ColumnId == entity.Id && toRemove.Contains(a.TaxonomyId))
                    .ExecuteDeleteAsync(ct);
            }

            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entity;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var entity = await _dbContext.Columns.FirstOrDefaultAsync(c => c.Id == id, ct);
            if (entity is null) return false;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;

            await _dbContext.ColumnTaxonomyAssociations
                .Where(a => a.ColumnId == entity.Id)
                .ExecuteDeleteAsync(ct);

            await _dbContext.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return true;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }

    public async Task<(IReadOnlyList<ColumnSummaryDto> Items, int TotalCount)> FetchAsync(
        long profileId,
        EnumTaxonomyType? category,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default
    )
    {
        var filteredQuery = from c in _dbContext.Columns
                    join a in _dbContext.ColumnTaxonomyAssociations
                    on c.Id equals a.ColumnId into ca
                    from a in ca.DefaultIfEmpty()
                    join t in _dbContext.ColumnTaxonomies
                    on a.TaxonomyId equals t.Id into tat
                    from t in tat.DefaultIfEmpty()
                    where c.ProfileId == profileId
                        && !c.IsDeleted
                        && !t.IsDeleted
                        && (category == null || (t != null && t.Type == category.Value))
                    select c;

        var total = await filteredQuery.CountAsync(ct);

        if (total == 0)
        {
            return (Array.Empty<ColumnSummaryDto>(), 0);
        }

        var orderedQuery = from c in filteredQuery
            orderby (c.PublishedAt ?? c.CreatedAt) descending, c.Id descending
            select c;

        // sanitize paging
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 20;
        const int MaxPageSize = 50;
        if (pageSize > MaxPageSize) pageSize = MaxPageSize;
        var skip = (page - 1) * pageSize;

        var result = await orderedQuery
            .Select(c => new ColumnSummaryDto(
                c.Id,
                c.Slug,
                c.Title,
                c.Summary,
                c.DisplayImage,
                c.IsPublished,
                c.CreatedAt,
                c.PublishedAt
            ))
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        var columnIds = result.ConvertAll(x => x.Id);
        var taxonomyQuery = from a in _dbContext.ColumnTaxonomyAssociations
                         join t in _dbContext.ColumnTaxonomies
                         on a.TaxonomyId equals t.Id
                         where !t.IsDeleted
                            && columnIds.Contains(a.ColumnId)
                            select new
                            {
                                a.ColumnId,
                                TaxonomyDto = new ColumnTaxonomySummaryDto(t.Id, t.Name, t.Type)
                            };

        var columnTaxnomoies = await taxonomyQuery
            .AsNoTracking()
            .ToListAsync(ct);

        foreach (var item in result)
        {
            item.Taxonomies = [.. columnTaxnomoies
                .Where(x => x.ColumnId == item.Id)
                .Select(x => x.TaxonomyDto)];
        }

        return (result, total);
    }

    public async Task<ColumnDetailDto?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        var result = await FindByPredicate(x => x.Id == id, ct);
        return result;
    }

    public async Task<ColumnDetailDto?> FindBySlugAsync(string slug, CancellationToken ct = default)
    {
        var result = await FindByPredicate(x => x.Slug.Equals(slug, StringComparison.CurrentCultureIgnoreCase), ct);
        return result;
    }

    private async Task<ColumnDetailDto?> FindByPredicate(Expression<Func<Column, bool>> predicate, CancellationToken ct = default)
    {
        var entity = await _dbContext.Columns
            .AsNoTracking()
            .FirstOrDefaultAsync(predicate, ct);
        if (entity is null) return null;

        var taxonomyQuery = from asso in _dbContext.ColumnTaxonomyAssociations
                            join taxo in _dbContext.ColumnTaxonomies
                            on asso.Id equals taxo.Id
                            where asso.ColumnId == entity.Id
                            select new ColumnTaxonomySummaryDto(taxo.Id, taxo.Name, taxo.Type);
        var taxonomyDtos = await taxonomyQuery.ToListAsync(ct);

        var result = new ColumnDetailDto(
            entity.Id,
            entity.Slug,
            entity.Title,
            entity.Summary,
            entity.Content,
            entity.DisplayImage,
            entity.IsPublished,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.PublishedAt,
            taxonomyDtos
        );

        return result;
    }
}
