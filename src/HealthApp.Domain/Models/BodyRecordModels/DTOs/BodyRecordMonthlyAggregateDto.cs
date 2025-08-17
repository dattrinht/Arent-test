namespace HealthApp.Domain.Models.BodyRecordModels.DTOs;

public sealed record BodyRecordMonthlyAggregateDto(
    int Year,
    int Month,
    float AverageWeight,
    float AverageBodyFat
);
