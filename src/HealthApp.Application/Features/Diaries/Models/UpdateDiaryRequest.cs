namespace HealthApp.Application.Features.Diaries.Models;

public sealed record UpdateDiaryRequest(
    [Required, StringLength(200, MinimumLength = 1)] string Title,
    [Required, StringLength(4000)] string Content,
    [Required] DateTime WrittenAt
);
