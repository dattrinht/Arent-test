namespace HealthApp.Application.Features.Diaries.Models;

public sealed record UpdateDiaryRequest(
    [property: Required, StringLength(200, MinimumLength = 1)] string Title,
    [property: Required, StringLength(4000)] string Content,
    [property: Required] DateTime WrittenAt
);
