using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record WalletDto(
    Guid Id,
    string Name,
    string Description,
    double Amount,
    CurrencyType Currency,
    WalletType WalletCategory,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    Guid User
);





