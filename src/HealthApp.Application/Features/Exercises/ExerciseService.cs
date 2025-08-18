namespace HealthApp.Application.Features.Exercises;

internal class ExerciseService : IExerciseService
{
    private readonly IExerciseRepository _exerciseRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ILogger<ExerciseService> _logger;

    public ExerciseService(
        IExerciseRepository exerciseRepository,
        IProfileRepository profileRepository,
        ILogger<ExerciseService> logger)
    {
        _exerciseRepository = exerciseRepository;
        _profileRepository = profileRepository;
        _logger = logger;
    }

    public async Task<CreateExerciseResponse> CreateAsync(CreateExerciseRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var _ = await _profileRepository.FindByIdAsync(req.ProfileId, ct) ?? throw new InvalidOperationException($"Profile {req.ProfileId} does not exist.");

        var now = DateTime.UtcNow;
        var entity = new Exercise
        {
            Id = IdGenHelper.CreateId(),
            ProfileId = req.ProfileId,
            Name = req.Name,
            Status = req.Status,
            Type = req.Type,
            CaloriesBurned = req.CaloriesBurned,
            DurationSec = req.DurationSec,
            FinishedAt = req.FinishedAt,
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };

        await _exerciseRepository.SaveAsync(entity, ct);
        _logger.LogInformation("Exercise created: {ExerciseId} for profile {ProfileId}", entity.Id, entity.ProfileId);

        return new CreateExerciseResponse(entity.Id, entity.ProfileId);
    }

    public async Task<ExerciseSummaryDto?> FindByIdAsync(long id, CancellationToken ct = default)
    {
        var entity = await _exerciseRepository.FindByIdAsync(id, ct);
        return entity is null ? null : new ExerciseSummaryDto(
            entity.Id,
            entity.ProfileId,
            entity.Name,
            entity.Type,
            entity.Status,
            entity.DurationSec,
            entity.CaloriesBurned,
            entity.FinishedAt
        );
    }

    public async Task<(IReadOnlyList<ExerciseSummaryDto> Items, long TotalCount)> FetchByProfileAsync(
        long profileId,
        DateOnly? byDate,
        int page,
        int pageSize,
        CancellationToken ct = default
    )
    {
        var spec = new ExercisesByProfileIdPagingSpec(profileId, byDate, page, pageSize);
        return await _exerciseRepository.FetchByProfileIdAsync(spec, ct);
    }

    public async Task<ExerciseSummaryDto?> UpdateAsync(long id, UpdateExerciseRequest req, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(req);

        var dto = new ExerciseSummaryDto(
            id,
            default,
            req.Name,
            req.Type,
            req.Status,
            req.DurationSec,
            req.CaloriesBurned,
            req.FinishedAt
        );

        var updated = await _exerciseRepository.UpdateAsync(dto, ct);
        if (updated is null) return null;

        _logger.LogInformation("Exercise updated: {ExerciseId}", id);

        return dto;
    }

    public async Task<bool> DeleteAsync(long id, CancellationToken ct = default)
    {
        var ok = await _exerciseRepository.DeleteAsync(id, ct);
        if (ok) _logger.LogInformation("Exercise soft-deleted: {ExerciseId}", id);
        return ok;
    }

    public async Task<ExerciseAchievementDto> GetAchievementByDateAsync(long profileId, DateOnly day, CancellationToken ct = default)
    {
        var result = await _exerciseRepository.GetAchievementAsync(profileId, day, null, ct);
        return result;
    }
}
