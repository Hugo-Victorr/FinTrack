using Dapper;
using FinTrack.Analytics.Contracts;
using FinTrack.Analytics.Models;
using FinTrack.Model.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace FinTrack.Analytics.Services
{
    public class DashboardService(IConfiguration configuration, ILogger<DashboardService> logger) : IDashboardService
    {
        private readonly string _connectionString = configuration.GetConnectionString("FintrackDb")
            ?? throw new InvalidOperationException("Connection string 'FintrackDb' not found.");

        public async Task<DashboardSummaryDTO> GetDashboardSummaryAsync(Guid userId, string period = "current")
        {
            var (startDate, endDate) = GetDateRange(period);
            var previousPeriodStart = GetPreviousPeriodStart(startDate, period);
            var previousPeriodEnd = startDate.AddDays(-1);

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Single optimized query for current period KPIs
            // Aggregate functions always return a row, even with no matching data
            // Expenses are stored as negative values, so we use ABS() for expenses
            var kpiQuery = @"
                SELECT
                    COALESCE(SUM(CASE WHEN ec.operation_type = 1 THEN e.amount ELSE 0 END), 0)::double precision as TotalIncome,
                    COALESCE(ABS(SUM(CASE WHEN ec.operation_type = 0 THEN e.amount ELSE 0 END)), 0)::double precision as TotalExpenses
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND e.expense_date >= @StartDate
                    AND e.expense_date <= @EndDate
                    AND e.user_id = @UserId";

            logger.LogInformation("Executing KPI query for UserId: {UserId}, Period: {Period}, StartDate: {StartDate}, EndDate: {EndDate}",
                userId, period, startDate, endDate);
            logger.LogDebug("KPI Query: {Query}", kpiQuery);

            var kpiResult = await connection.QueryFirstOrDefaultAsync<KpiResult>(kpiQuery, new
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId
            });

            // Aggregate queries should always return a row, but handle null just in case
            if (kpiResult == null)
            {
                kpiResult = new KpiResult { TotalIncome = 0, TotalExpenses = 0 };
            }

            var totalIncome = kpiResult.TotalIncome;
            var totalExpenses = kpiResult.TotalExpenses;

            // Previous period KPIs
            // Expenses are stored as negative values, so we use ABS() for expenses
            var previousKpiQuery = @"
                SELECT
                    COALESCE(SUM(CASE WHEN ec.operation_type = 1 THEN e.amount ELSE 0 END), 0)::double precision as TotalIncome,
                    COALESCE(ABS(SUM(CASE WHEN ec.operation_type = 0 THEN e.amount ELSE 0 END)), 0)::double precision as TotalExpenses
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND e.expense_date >= @PreviousStart
                    AND e.expense_date <= @PreviousEnd
                    AND e.user_id = @UserId";

            logger.LogInformation("Executing Previous Period KPI query for UserId: {UserId}, PreviousStart: {PreviousStart}, PreviousEnd: {PreviousEnd}",
                userId, previousPeriodStart, previousPeriodEnd);
            logger.LogDebug("Previous KPI Query: {Query}", previousKpiQuery);

            var previousKpiResult = await connection.QueryFirstOrDefaultAsync<KpiResult>(previousKpiQuery, new
            {
                PreviousStart = previousPeriodStart,
                PreviousEnd = previousPeriodEnd,
                UserId = userId
            });

            if (previousKpiResult == null)
            {
                previousKpiResult = new KpiResult { TotalIncome = 0, TotalExpenses = 0 };
            }

            var previousTotalIncome = previousKpiResult.TotalIncome;
            var previousTotalExpenses = previousKpiResult.TotalExpenses;

            // Total balance from wallets
            var balanceQuery = @"
                SELECT COALESCE(SUM(w.amount), 0)::double precision as TotalBalance
                FROM wallet w
                WHERE w.deleted_at IS NULL
                    AND w.user_id = @UserId";

            logger.LogInformation("Executing Balance query for UserId: {UserId}", userId);
            logger.LogDebug("Balance Query: {Query}", balanceQuery);

            var balanceResult = await connection.QueryFirstOrDefaultAsync<BalanceResult>(balanceQuery, new
            {
                UserId = userId
            });

            // SUM always returns a row (with NULL if no rows), COALESCE converts to 0
            if (balanceResult == null)
            {
                balanceResult = new BalanceResult { TotalBalance = 0 };
            }
            var totalBalance = balanceResult.TotalBalance;

            // Calculate balance at end of current period and previous period
            // Balance at end of current period = current balance - transactions after endDate
            // Balance at end of previous period = balance at end of current period - transactions in current period
            
            // Sum of transactions after endDate
            var transactionsAfterEndDateQuery = @"
                SELECT COALESCE(SUM(e.amount), 0)::double precision as TotalBalance
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND e.expense_date > @EndDate
                    AND e.user_id = @UserId";

            logger.LogInformation("Executing Transactions After EndDate query for UserId: {UserId}, EndDate: {EndDate}",
                userId, endDate);
            logger.LogDebug("Transactions After EndDate Query: {Query}", transactionsAfterEndDateQuery);

            var transactionsAfterEndDateResult = await connection.QueryFirstOrDefaultAsync<BalanceResult>(
                transactionsAfterEndDateQuery, new
                {
                    EndDate = endDate,
                    UserId = userId
                });

            var transactionsAfterEndDate = transactionsAfterEndDateResult?.TotalBalance ?? 0;

            // Sum of transactions in current period
            var transactionsInCurrentPeriodQuery = @"
                SELECT COALESCE(SUM(e.amount), 0)::double precision as TotalBalance
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND e.expense_date >= @StartDate
                    AND e.expense_date <= @EndDate
                    AND e.user_id = @UserId";

            logger.LogInformation("Executing Transactions In Current Period query for UserId: {UserId}, StartDate: {StartDate}, EndDate: {EndDate}",
                userId, startDate, endDate);
            logger.LogDebug("Transactions In Current Period Query: {Query}", transactionsInCurrentPeriodQuery);

            var transactionsInCurrentPeriodResult = await connection.QueryFirstOrDefaultAsync<BalanceResult>(
                transactionsInCurrentPeriodQuery, new
                {
                    StartDate = startDate,
                    EndDate = endDate,
                    UserId = userId
                });

            var transactionsInCurrentPeriod = transactionsInCurrentPeriodResult?.TotalBalance ?? 0;

            // Calculate balances at end of each period
            var balanceAtEndOfCurrentPeriod = totalBalance - transactionsAfterEndDate;
            var balanceAtEndOfPreviousPeriod = balanceAtEndOfCurrentPeriod - transactionsInCurrentPeriod;

            // Calculate changes
            var incomeChange = previousTotalIncome > 0
                ? ((totalIncome - previousTotalIncome) / previousTotalIncome) * 100
                : (totalIncome > 0 ? 100 : 0);
            var expenseChange = previousTotalExpenses > 0
                ? ((totalExpenses - previousTotalExpenses) / previousTotalExpenses) * 100
                : (totalExpenses > 0 ? 100 : 0);

            // Calculate balance change based on wallet balances at end of each period
            var balanceChange = balanceAtEndOfPreviousPeriod != 0
                ? ((balanceAtEndOfCurrentPeriod - balanceAtEndOfPreviousPeriod) / Math.Abs(balanceAtEndOfPreviousPeriod)) * 100
                : (balanceAtEndOfCurrentPeriod != 0 ? (balanceAtEndOfCurrentPeriod > 0 ? 100 : -100) : 0);
            var deviation = totalIncome > 0 ? ((totalIncome - totalExpenses) / totalIncome) * 100 : 0;
            var monthlyBalance = totalIncome - totalExpenses;
            var incomePercentage = totalIncome > 0 ? 100.0 : 0.0;

            // Query top 3 biggest incomes by category
            var topIncomesQuery = @"
                SELECT
                    COALESCE(ec.description, 'Sem categoria') as Category,
                    ec.color as CategoryColor,
                    SUM(e.amount) as Amount
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND ec.operation_type = 1
                    AND e.expense_date >= @StartDate
                    AND e.expense_date <= @EndDate
                    AND e.user_id = @UserId
                GROUP BY ec.description, ec.color
                ORDER BY SUM(e.amount) DESC
                LIMIT 3";

            logger.LogInformation("Executing Top Incomes query for UserId: {UserId}, StartDate: {StartDate}, EndDate: {EndDate}",
                userId, startDate, endDate);
            logger.LogDebug("Top Incomes Query: {Query}", topIncomesQuery);

            var topIncomesResults = await connection.QueryAsync<TopIncomeItemDTO>(topIncomesQuery, new
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId
            });
            var topIncomes = topIncomesResults?.ToList() ?? new List<TopIncomeItemDTO>();
            
            // Calculate percentage for each top income item
            if (totalIncome > 0)
            {
                foreach (var income in topIncomes)
                {
                    income.Percentage = (income.Amount / totalIncome) * 100;
                }
            }

            // Build typed KPI DTOs with raw values only
            var consolidatedBalance = new ConsolidatedBalanceKpiDTO
            {
                Value = totalBalance,
                BalanceChange = balanceChange
            };

            var totalIncomeKpi = new TotalIncomeKpiDTO
            {
                Value = totalIncome,
                IncomePercentage = incomePercentage,
                TopIncomes = topIncomes
            };

            var totalExpensesKpi = new TotalExpensesKpiDTO
            {
                Value = totalExpenses,
                ExpenseChange = expenseChange
            };

            var incomeExpenseDeviation = new IncomeExpenseDeviationKpiDTO
            {
                Value = deviation,
                MonthlyBalance = monthlyBalance
            };

            // Weekly data using SQL aggregation
            // Expenses are stored as negative values, so we use ABS() for expenses
            var weeklyDataQuery = @"
                WITH week_numbers AS (
                    SELECT
                        e.expense_date,
                        ec.operation_type,
                        e.amount,
                        FLOOR(EXTRACT(EPOCH FROM (e.expense_date - @StartDate)) / (7 * 24 * 60 * 60))::int + 1 as week_number
                    FROM expense e
                    INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                    WHERE e.deleted_at IS NULL
                        AND ec.deleted_at IS NULL
                        AND e.expense_date >= @StartDate
                        AND e.expense_date <= @EndDate
                        AND e.user_id = @UserId
                )
                SELECT
                    'Semana ' || week_number::text as Week,
                    CASE WHEN operation_type = 1 THEN 'Receitas' ELSE 'Despesas' END as Type,
                    CASE 
                        WHEN operation_type = 1 THEN SUM(amount)
                        ELSE ABS(SUM(amount))
                    END as Value
                FROM week_numbers
                GROUP BY week_number, operation_type
                HAVING (operation_type = 1 AND SUM(amount) > 0) OR (operation_type = 0 AND SUM(amount) < 0)
                ORDER BY week_number, operation_type";

            logger.LogInformation("Executing Weekly Data query for UserId: {UserId}, StartDate: {StartDate}, EndDate: {EndDate}",
                userId, startDate, endDate);
            logger.LogDebug("Weekly Data Query: {Query}", weeklyDataQuery);

            var weeklyDataResults = await connection.QueryAsync<WeeklyFinancePointDTO>(weeklyDataQuery, new
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId
            });
            var weeklyData = weeklyDataResults?.ToList() ?? new List<WeeklyFinancePointDTO>();

            // Wallets
            var walletsQuery = @"
                SELECT
                    w.name as Name,
                    w.amount as Balance,
                    CASE w.wallet_category
                        WHEN 0 THEN 'Dinheiro'
                        WHEN 1 THEN 'Carteira Digital'
                        WHEN 2 THEN 'Cartão de Crédito'
                        WHEN 3 THEN 'Conta Poupança'
                        ELSE 'Outro'
                    END as Type
                FROM wallet w
                WHERE w.deleted_at IS NULL
                    AND w.user_id = @UserId
                ORDER BY w.amount DESC";

            logger.LogInformation("Executing Wallets query for UserId: {UserId}", userId);
            logger.LogDebug("Wallets Query: {Query}", walletsQuery);

            var walletResults = await connection.QueryAsync<WalletItemDTO>(walletsQuery, new
            {
                UserId = userId
            });
            var walletItems = walletResults?.ToList() ?? new List<WalletItemDTO>();

            // Recent transactions
            // Expenses are stored as negative values, so we use ABS() for expenses
            // Incomes are stored as positive values, so we use them directly
            var transactionsQuery = @"
                SELECT
                    TO_CHAR(e.expense_date, 'YYYY-MM-DD') as Date,
                    COALESCE(ec.description, 'Sem categoria') as Category,
                    ec.color as CategoryColor,
                    e.description as Description,
                    e.amount as Value,
                    COALESCE(w.name, 'Sem carteira') as Wallet
                FROM expense e
                INNER JOIN expense_category ec ON e.expense_category_id = ec.id
                LEFT JOIN wallet w ON e.wallet_id = w.id
                WHERE e.deleted_at IS NULL
                    AND ec.deleted_at IS NULL
                    AND e.expense_date >= @StartDate
                    AND e.expense_date <= @EndDate
                    AND e.user_id = @UserId
                ORDER BY e.expense_date DESC, e.created_at DESC
                LIMIT 10";

            logger.LogInformation("Executing Recent Transactions query for UserId: {UserId}, StartDate: {StartDate}, EndDate: {EndDate}",
                userId, startDate, endDate);
            logger.LogDebug("Transactions Query: {Query}", transactionsQuery);

            var transactionResults = await connection.QueryAsync<TransactionItemDTO>(transactionsQuery, new
            {
                StartDate = startDate,
                EndDate = endDate,
                UserId = userId
            });
            var recentTransactions = transactionResults?.ToList() ?? new List<TransactionItemDTO>();

            return new DashboardSummaryDTO
            {
                ConsolidatedBalance = consolidatedBalance,
                TotalIncome = totalIncomeKpi,
                TotalExpenses = totalExpensesKpi,
                IncomeExpenseDeviation = incomeExpenseDeviation,
                WeeklyData = weeklyData,
                Wallets = walletItems,
                RecentTransactions = recentTransactions
            };
        }

        private (DateTime startDate, DateTime endDate) GetDateRange(string period)
        {
            var now = DateTime.Now;
            return period switch
            {
                "current" => (new DateTime(now.Year, now.Month, 1), now),
                "3m" => (now.AddMonths(-3), now),
                "6m" => (now.AddMonths(-6), now),
                _ => (new DateTime(now.Year, now.Month, 1), now)
            };
        }

        private DateTime GetPreviousPeriodStart(DateTime currentStart, string period)
        {
            return period switch
            {
                "current" => currentStart.AddMonths(-1),
                "3m" => currentStart.AddMonths(-3),
                "6m" => currentStart.AddMonths(-6),
                _ => currentStart.AddMonths(-1)
            };
        }
    }
}
