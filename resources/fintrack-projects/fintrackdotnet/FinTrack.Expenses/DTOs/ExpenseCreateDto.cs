namespace FinTrack.Expenses.DTOs;

public record ExpenseCreateDto(
    string Description,
    Guid ExpenseCategoryId,
    double Amount,
    DateTime ExpenseDate,
    Guid WalletId
);
