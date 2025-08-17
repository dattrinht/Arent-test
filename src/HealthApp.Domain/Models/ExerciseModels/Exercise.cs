namespace HealthApp.Domain.Models.ExerciseModels;

public class Exercise : Entity
{
    public long ProfileId { get; set; }
    public string Name { get; set; }
    public short Type { get; set; }
    public int DurationSec { get; set; }
    public int CaloriesBurned { get; set; }
    public DateTime DoneAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
