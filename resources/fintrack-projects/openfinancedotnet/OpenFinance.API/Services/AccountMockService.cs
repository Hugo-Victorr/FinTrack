using OpenFinance.Model.Entities;
using OpenFinance.Model.Enums;

namespace OpenFinance.API.Services
{
    /// <summary>
    /// Serviço de mock para dados de contas.
    /// </summary>
    public static class AccountMockService
    {
        /// <summary>
        /// Retorna uma lista mockada de contas associadas aos clientes e organizações financeiras.
        /// </summary>
        /// <returns>Lista de contas</returns>
        public static List<Account> GetMockAccounts()
        {
            var random = new Random();
            var costumers = CostumerMockService.GetMockCostumers();
            var financialOrganizations = FinancialOrganizationMockService.GetMockFinancialOrganizations();
            var accounts = new List<Account>();

            var accountTypes = new[] { AccountTypeEnum.CACC, AccountTypeEnum.SVGS, AccountTypeEnum.SLRY, AccountTypeEnum.CCRD };
            var currencies = new[] { CurrencyTypeEnum.BRL };

            int accountId = 1;

            foreach (var costumer in costumers)
            {
                // Cada cliente terá entre 1 e 4 contas
                int numberOfAccounts = random.Next(1, 5);

                for (int i = 0; i < numberOfAccounts; i++)
                {
                    var randomFinancialOrg = financialOrganizations[random.Next(financialOrganizations.Count)];
                    var randomAccountType = accountTypes[random.Next(accountTypes.Length)];
                    var randomCurrency = currencies[random.Next(currencies.Length)];
                    var randomStatus = (AccountStatusEnum)random.Next(0, 4);
                    var accountNumber = (1000000 + accountId).ToString();
                    var branchCode = (random.Next(1, 10000)).ToString("D4");
                    var openingDate = new DateTime(random.Next(2015, 2024), random.Next(1, 13), random.Next(1, 29));
                    var amount = Math.Round((decimal)(random.NextDouble() * 50000), 2); // até 50k

                    accounts.Add(new Account
                    {
                        Id = Guid.NewGuid(),
                        CustomerId = costumer.Id,
                        Currency = randomCurrency,
                        Type = randomAccountType,
                        BranchCode = branchCode,
                        Number = accountNumber,
                        Status = randomStatus,
                        OpeningDate = openingDate,
                        Financial = randomFinancialOrg,
                        Amount = amount,
                        CreatedAt = DateTime.Now.ToLocalTime()
                    });

                    accountId++;
                }
            }

            return accounts;
        }

        /// <summary>
        /// Retorna todas as contas mockadas de um cliente específico.
        /// </summary>
        /// <param name="customerId">ID do cliente</param>
        /// <returns>Lista de contas do cliente</returns>
        public static List<Account> GetMockAccountsByCustomerId(Guid customerId)
        {
            return GetMockAccounts().Where(x => x.CustomerId == customerId).ToList();
        }

        /// <summary>
        /// Retorna uma conta mockada pelo ID.
        /// </summary>
        /// <param name="accountId">ID da conta</param>
        /// <returns>Conta ou null se não encontrada</returns>
        public static Account? GetMockAccountById(Guid accountId)
        {
            return GetMockAccounts().FirstOrDefault(x => x.Id == accountId);
        }

        /// <summary>
        /// Retorna todas as contas mockadas de uma organização financeira específica.
        /// </summary>
        /// <param name="financialOrganizationId">ID da organização financeira</param>
        /// <returns>Lista de contas da organização</returns>
        public static List<Account> GetMockAccountsByFinancialOrganization(Guid financialOrganizationId)
        {
            return GetMockAccounts().Where(x => x.Financial != null && x.Financial.Id == financialOrganizationId).ToList();
        }

        /// <summary>
        /// Retorna todas as contas mockadas de um tipo específico.
        /// </summary>
        /// <param name="accountType">Tipo de conta</param>
        /// <returns>Lista de contas do tipo especificado</returns>
        public static List<Account> GetMockAccountsByType(AccountTypeEnum accountType)
        {
            return GetMockAccounts().Where(x => x.Type == accountType).ToList();
        }
    }
}
