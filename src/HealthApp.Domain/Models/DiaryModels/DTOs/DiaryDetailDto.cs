namespace HealthApp.Domain.Models.DiaryModels.DTOs;

public sealed record DiaryDetailDto(
    long Id,
    long ProfileId,
    string Title,
    string Content,
    string Preview,
    DateTime WrittenAt,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
