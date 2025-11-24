using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record ExpenseCategoryUpdateDto(
    string Description,
    string Color,
    OperationType OperationType
);





