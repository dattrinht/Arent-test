namespace HealthApp.Domain.Models.DiarieModels;

public class Diarie : Entity
{
    public long ProfileId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DoneAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
