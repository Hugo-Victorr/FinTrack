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
export interface KpiCard {
  title: string;
  value: number;
  prefix?: string;
  suffix?: string;
  subtitle?: string;
  tag?: {
    color: string;
    label: string;
  };
  decimals?: number;
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
  description: string;
  value: number;
  wallet: string;
}
