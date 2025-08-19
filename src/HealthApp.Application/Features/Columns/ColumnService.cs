namespace HealthApp.Application.Features.Columns;

internal class ColumnService : IColumnService
{
    private readonly IColumnRepository _columnRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ColumnService> _logger;

    public ColumnService(
        IColumnRepository columnRepository,
        IProfileRepository profileRepository,
        ILogger<ColumnService> logger)
    {
        _columnRepository = columnRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateColumnResponse> CreateAsync(CreateColumnRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var profile = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var dto = new ColumnDetailDto(
            Id: 0,
            ProfileId: req.ProfileId,
            Slug: (req.Slug ?? string.Empty).Trim().ToLowerInvariant(),
            Title: (req.Title ?? string.Empty).Trim(),
            Summary: req.Summary ?? string.Empty,
            Content: req.Content ?? string.Empty,
            DisplayImage: string.IsNullOrWhiteSpace(req.DisplayImage) ? null : req.DisplayImage.Trim(),
            IsPublished: req.IsPublished,
            CreatedAt: default,     // ignored by repo
            UpdatedAt: default,     // ignored by repo
            PublishedAt: default,   // ignored by repo
            Taxonomies: [.. (req.TaxonomyIds ?? [])
                .Distinct()
                .Select(id => new ColumnTaxonomySummaryDto(id, string.Empty, default))]
        );

        var entity = await _columnRepository.CreateAsync(dto, ct);

        _logger.LogInformation("Column created: {ColumnId} ({Slug}) for profile {ProfileId}", entity.Id, entity.Slug, profile.Id);

        return new CreateColumnResponse(entity.Id, entity.ProfileId);
    }

    public async Task<ColumnDetailDto?> UpdateAsync(long id, UpdateColumnRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var dto = new ColumnDetailDto(
            Id: id,
            ProfileId: default,    // ignored by repo
            Slug: (req.Slug ?? string.Empty).Trim().ToLowerInvariant(),
            Title: (req.Title ?? string.Empty).Trim(),
            Summary: req.Summary ?? string.Empty,
            Content: req.Content ?? string.Empty,
            DisplayImage: string.IsNullOrWhiteSpace(req.DisplayImage) ? null : req.DisplayImage.Trim(),
            IsPublished: req.IsPublished,
            CreatedAt: default,     // ignored by repo
            UpdatedAt: default,     // ignored by repo
            PublishedAt: default,   // ignored by repo
            Taxonomies: [.. (req.TaxonomyIds ?? [])
                .Distinct()
                .Select(id => new ColumnTaxonomySummaryDto(id, string.Empty, default))]
        );

        var updated = await _columnRepository.UpdateAsync(dto, ct);
        if (updated is null) return null;

        _logger.LogInformation("Column updated: {ColumnId} ({Slug})", updated.Id, updated.Slug);

        return dto;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var ok = await _columnRepository.DeleteAsync(id, ct);
        if (ok) _logger.LogInformation("Column soft-deleted: {ColumnId}", id);
        return ok;
    }

    public async Task<(IReadOnlyList<ColumnSummaryDto> Items, int TotalCount)> FetchAsync(long profileId, EnumTaxonomyType? category, int page = 1, int pageSize = 20, CancellationToken ct = default)
    {
        var result = await _columnRepository.FetchAsync(profileId, category, page, pageSize, ct);
        return result;
    }

    public async Task<ColumnDetailDto?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        var result = await _columnRepository.FindByIdAsync(id, ct);
        return result;
    }

    public async Task<ColumnDetailDto?> FindBySlugAsync(string slug, CancellationToken ct = default)
    {
        var result = await _columnRepository.FindBySlugAsync(slug, ct);
        return result;
    }
}
