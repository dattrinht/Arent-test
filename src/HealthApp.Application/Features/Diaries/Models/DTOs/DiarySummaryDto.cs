namespace HealthApp.Application.Features.Diaries.Models.DTOs;

public sealed record DiarySummaryDto(
    long Id,
    long ProfileId,
    string Title,
    string Preview,
    DateTime WrittenAt
);