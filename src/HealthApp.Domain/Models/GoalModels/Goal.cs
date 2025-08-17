namespace HealthApp.Domain.Models.GoalModels;

public class Goal : Entity
{
    public long ProfileId { get; set; }
    public string Name { get; set; }
    public short Type { get; set; }
    public short Status { get; set; }
    public DateTime DoneAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
