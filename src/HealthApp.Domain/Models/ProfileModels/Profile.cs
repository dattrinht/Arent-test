namespace HealthApp.Domain.Models.ProfileModels;

public class Profile : Entity
{
    public long UserId { get; set; }
    public string FisrtName { get; set; }
    public string LastName { get; set; }
    public DateOnly Birthday { get; set; }
    public short Sex { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
