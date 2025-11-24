namespace FinTrack.Expenses.DTOs;

public record ExpenseUpdateDto(
    string Description,
    Guid ExpenseCategoryId,
    double Amount,
    DateTime ExpenseDate,
    Guid WalletId
);





