namespace HealthApp.Domain.Models.BodyRecordModels;

public class BodyRecord : Entity
{
    public long ProfileId { get; set; }
    public float Title { get; set; }
    public string Weight { get; set; }
    public string BodyFat { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
