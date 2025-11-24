using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record ExpenseCategoryCreateDto(
    string Description,
    string Color,
    OperationType OperationType
);





