namespace HealthApp.Domain.Models.MealModels;

public class Meal : Entity
{
    [Required]
    public long ProfileId { get; set; }

    [MaxLength(128)]
    public string? Name { get; set; }

    [Required]
    public EnumMealType Type { get; set; }

    [MaxLength(256)]
    public string? Image { get; set; }

    public DateTime DoneAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
}
