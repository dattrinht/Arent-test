namespace HealthApp.Domain.Models.IdentityModels;

public class User : Entity
{
    [Required]
    [EmailAddress]
    [MaxLength(64)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(512)]
    public required string PasswordHash { get; set; }

    [Required]
    [MaxLength(128)]
    public required string PasswordSalt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
