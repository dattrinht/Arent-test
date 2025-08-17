namespace HealthApp.Application.Features.Diaries.Models;

public sealed record CreateDiaryRequest(
    [Required] long ProfileId,
    [Required, StringLength(200)] string Title,
    [Required, StringLength(4000)] string Content,
    [Required] DateTime WrittenAt
);

