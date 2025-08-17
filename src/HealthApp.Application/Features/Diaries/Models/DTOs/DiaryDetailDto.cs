namespace HealthApp.Application.Features.Diaries.Models.DTOs;

public sealed record DiaryDetailDto(
    long Id,
    long ProfileId,
    string Title,
    string Content,
    string Preview,
    DateTime DoneAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
