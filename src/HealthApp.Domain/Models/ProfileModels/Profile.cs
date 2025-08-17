namespace HealthApp.Domain.Models.ProfileModels;

public class Profile : Entity
{
    [Required]
    public long UserId { get; set; }

    [Required]
    [MaxLength(64)]
    public required string FirstName { get; set; }

    [Required]
    [MaxLength(64)]
    public required string LastName { get; set; }

    [Required]
    public DateOnly Birthday { get; set; }

    [Required]
    public EnumSex Sex { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
