namespace FinTrack.Expenses.DTOs;

public record ExpenseDto(
    Guid Id,
    string Description,
    Guid ExpenseCategoryId,
    ExpenseCategoryDto? ExpenseCategory,
    double Amount,
    DateTime ExpenseDate,
    Guid WalletId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid User
);





