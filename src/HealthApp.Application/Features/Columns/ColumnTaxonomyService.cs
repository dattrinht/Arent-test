namespace HealthApp.Application.Features.Columns;

internal class ColumnTaxonomyService : IColumnTaxonomyService
{
    private readonly IColumnTaxonomyRepository _columnTaxonomyRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ColumnTaxonomyService> _logger;

    public ColumnTaxonomyService(
        IColumnTaxonomyRepository columnTaxonomyRepository,
        IProfileRepository profileRepository,
        ILogger<ColumnTaxonomyService> logger)
    {
        _columnTaxonomyRepository = columnTaxonomyRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateColumnTaxonomyResponse> CreateAsync(CreateColumnTaxonomyRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var entity = new ColumnTaxonomy
        {
            Id = IdGenHelper.CreateId(),
            ProfileId = req.ProfileId,
            Name = req.Name,
            Type = req.Type,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _columnTaxonomyRepository.SaveAsync(entity, ct);
        _logger.LogInformation("ColumnTaxonomy created: {ColumnTaxonomyId} for profile {ProfileId}", entity.Id, entity.ProfileId);

        return new CreateColumnTaxonomyResponse(entity.Id, entity.ProfileId);
    }

    public async Task<(IReadOnlyList<ColumnTaxonomySummaryDto> Items, long TotalCount)> FetchByProfileAsync(
        long profileId,
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var spec = new ColumnTaxonomyByProfileIdPagingSpec(profileId, page, pageSize);
        var result = await _columnTaxonomyRepository.FetchByProfileIdAsync(spec, ct);
        return result;
    }

    public async Task<ColumnTaxonomySummaryDto?> UpdateAsync(long id, UpdateColumnTaxonomyRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var dto = new ColumnTaxonomySummaryDto(id, req.Name, default);

        var updated = await _columnTaxonomyRepository.UpdateAsync(dto, ct);
        if (updated is null) return null;

        _logger.LogInformation("ColumnTaxonomy updated: {ColumnTaxonomyId}", id);

        return dto;
    }
}
