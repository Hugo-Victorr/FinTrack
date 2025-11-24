namespace FinTrack.Model.DTO
{
    public class DashboardSummaryDTO
    {
        public ConsolidatedBalanceKpiDTO ConsolidatedBalance { get; set; } = new();
        public TotalIncomeKpiDTO TotalIncome { get; set; } = new();
        public TotalExpensesKpiDTO TotalExpenses { get; set; } = new();
        public IncomeExpenseDeviationKpiDTO IncomeExpenseDeviation { get; set; } = new();
        public List<WeeklyFinancePointDTO> WeeklyData { get; set; } = new();
        public List<WalletItemDTO> Wallets { get; set; } = new();
        public List<TransactionItemDTO> RecentTransactions { get; set; } = new();
    }

    public class ConsolidatedBalanceKpiDTO
    {
        public double Value { get; set; }
        public double BalanceChange { get; set; }
    }

    public class TotalIncomeKpiDTO
    {
        public double Value { get; set; }
        public double IncomePercentage { get; set; }
        public List<TopIncomeItemDTO> TopIncomes { get; set; } = new();
    }

    public class TopIncomeItemDTO
    {
        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public double Amount { get; set; }
        public double Percentage { get; set; }
    }   

    public class TotalExpensesKpiDTO
    {
        public double Value { get; set; }
        public double ExpenseChange { get; set; }
    }

    public class IncomeExpenseDeviationKpiDTO
    {
        public double Value { get; set; }
        public double MonthlyBalance { get; set; }
    }

    public class WeeklyFinancePointDTO
    {
        public string Week { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // "Receitas" or "Despesas"
        public double Value { get; set; }
    }

    public class WalletItemDTO
    {
        public string Name { get; set; } = string.Empty;
        public double Balance { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public class TransactionItemDTO
    {
        public string Date { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Value { get; set; }
        public string Wallet { get; set; } = string.Empty;
    }
}







