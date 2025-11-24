using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record WalletUpdateDto(
    string Name,
    string Description,
    double Amount,
    CurrencyType Currency,
    WalletType WalletCategory
);





