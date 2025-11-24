export interface Wallet {
  id: string;
  name: string;
  description: string;
  amount: number;
  currency: number; // CurrencyType enum: 0=USD, 1=BRL, 2=EUR, 3=GBP, 4=JPY
  walletCategory: number; // WalletType enum: 0=Cash, 1=DigitalWallet, 2=CreditCard, 3=SavingsAccount
  createdAt: string;
  updatedAt?: string;
  user: string;
}

export const CurrencyType = {
  USD: 0,
  BRL: 1,
  EUR: 2,
  GBP: 3,
  JPY: 4,
} as const;

export const CurrencyLabels: Record<number, string> = {
  0: "USD",
  1: "BRL",
  2: "EUR",
  3: "GBP",
  4: "JPY",
};

export const WalletType = {
  Cash: 0,
  DigitalWallet: 1,
  CreditCard: 2,
  SavingsAccount: 3,
} as const;

export const WalletTypeLabels: Record<number, string> = {
  0: "Cash",
  1: "Digital Wallet",
  2: "Credit Card",
  3: "Savings Account",
};







