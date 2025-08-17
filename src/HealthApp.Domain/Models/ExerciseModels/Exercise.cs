namespace HealthApp.Domain.Models.ExerciseModels;

public class Exercise : Entity
{
    [Required]
    public long ProfileId { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [Required]
    public EnumExerciseType Type { get; set; } 

    [Required]
    public EnumExerciseStatus Status { get; set; }

    [Range(0, int.MaxValue)]
    public int DurationSec { get; set; }

    [Range(0, int.MaxValue)]
    public int CaloriesBurned { get; set; }

    [Required]
    public DateTime FinishedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
