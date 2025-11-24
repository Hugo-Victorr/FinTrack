using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record ExpenseCategoryDto(
    Guid Id,
    string Description,
    string Color,
    OperationType OperationType,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid User
);





