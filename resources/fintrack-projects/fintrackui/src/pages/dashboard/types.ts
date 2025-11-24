// ./types/finance-dashboard.ts

// ---- Filters ----
export interface PeriodOption {
  label: string;
  value: string;
}

export interface CurrencyOption {
  label: string;
  value: string;
}

// ---- KPI Cards ----
export interface ConsolidatedBalanceKpi {
  value: number;
  balanceChange: number;
}

export interface TopIncomeItem {
  category: string;
  categoryColor: string;
  percentage: number;
}

export interface TotalIncomeKpi {
  value: number;
  incomePercentage: number;
  topIncomes: TopIncomeItem[];
}

export interface TotalExpensesKpi {
  value: number;
  expenseChange: number;
}

export interface IncomeExpenseDeviationKpi {
  value: number;
  monthlyBalance: number;
}

// ---- Chart ----
export interface WeeklyFinancePoint {
  week: string;
  type: "Receitas" | "Despesas";
  value: number;
  color?: string;
}

// ---- Wallets ----
export interface WalletItem {
  name: string;
  balance: number;
  type: string;
}

// ---- Transactions ----
export interface TransactionItem {
  date: string;
  category: string;
  categoryColor: string;
  description: string;
  value: number;
  wallet: string;
}

export interface DashboardResponse {
  consolidatedBalance: ConsolidatedBalanceKpi;
  totalIncome: TotalIncomeKpi;
  totalExpenses: TotalExpensesKpi;
  incomeExpenseDeviation: IncomeExpenseDeviationKpi;
  weeklyData: WeeklyFinancePoint[];
  wallets: WalletItem[];
  recentTransactions: TransactionItem[];
}