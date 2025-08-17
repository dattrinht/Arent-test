namespace HealthApp.Domain.Models.BodyRecordModels;

public class BodyRecord : Entity
{
    [Required]
    public long ProfileId { get; set; }

    public string? Title { get; set; }

    [Required]
    public float Weight { get; set; }

    [Required]
    public float BodyFat { get; set; }

    [Required]
    public DateOnly RecordedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
