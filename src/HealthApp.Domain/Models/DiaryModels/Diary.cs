namespace HealthApp.Domain.Models.DiaryModels;

public class Diary : Entity
{
    [Required]
    public long ProfileId { get; set; }

    [Required]
    [MaxLength(256)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(4000)]
    public required string Content { get; set; }

    [Required]
    [MaxLength(512)]
    public string Preview { get; set; } = string.Empty;

    [Required]
    public DateTime WrittenAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
