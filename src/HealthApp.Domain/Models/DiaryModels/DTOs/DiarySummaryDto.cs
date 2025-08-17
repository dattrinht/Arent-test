namespace HealthApp.Domain.Models.DiaryModels.DTOs;

public sealed record DiarySummaryDto(
    long Id,
    long ProfileId,
    string Title,
    string Preview,
    DateTime WrittenAt
);