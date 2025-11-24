export interface Expense {
  id: string;
  description: string;
  expenseCategoryId: string;
  expenseCategory?: ExpenseCategory;
  amount: number;
  expenseDate: string;
  walletId: string;
  createdAt: string;
  updatedAt?: string;
  user: string;
}

export interface ExpenseCategory {
  id: string;
  description: string;
  color: string;
  operationType: number; // OperationType enum: 0=Expense, 1=Income
  createdAt: string;
  updatedAt?: string;
  user: string;
}

export const OperationType = {
  Expense: 0,
  Income: 1,
} as const;

export interface Wallet {
  id: string;
  name: string;
  description: string;
  amount: number;
  currency: number;
  walletCategory: number;
  createdAt: string;
  updatedAt?: string;
  user: string;
}







