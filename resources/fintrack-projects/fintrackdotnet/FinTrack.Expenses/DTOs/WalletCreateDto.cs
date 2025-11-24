using FinTrack.Model.Enums;

namespace FinTrack.Expenses.DTOs;

public record WalletCreateDto(
    string Name,
    string Description,
    double Amount,
    CurrencyType Currency,
    WalletType WalletCategory
);





