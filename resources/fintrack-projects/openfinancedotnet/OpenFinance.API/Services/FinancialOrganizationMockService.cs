using OpenFinance.Model.Entities;

namespace OpenFinance.API.Services
{
    /// <summary>
    /// Serviço de mock para dados de organizações financeiras.
    /// </summary>
    public static class FinancialOrganizationMockService
    {
        /// <summary>
        /// Retorna uma lista mockada de organizações financeiras brasileiras.
        /// </summary>
        /// <returns>Lista de organizações financeiras</returns>
        public static List<FinancialOrganization> GetMockFinancialOrganizations()
        {
            return new List<FinancialOrganization>
            {
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    BrandName = "Nubank",
                    Name = "Nubank Brasil Ltda.",
                    Cnpj = "27865757000102",
                    WebPath = "https://www.nubank.com.br"
                },
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                    BrandName = "Santander",
                    Name = "Banco Santander (Brasil) S.A.",
                    Cnpj = "90400888000123",
                    WebPath = "https://www.santander.com.br"
                },
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                    BrandName = "Itaú",
                    Name = "Banco Itaú S.A.",
                    Cnpj = "60701190000104",
                    WebPath = "https://www.itau.com.br"
                },
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                    BrandName = "Bradesco",
                    Name = "Banco Bradesco S.A.",
                    Cnpj = "60746948000125",
                    WebPath = "https://www.bradesco.com.br"
                },
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                    BrandName = "Caixa",
                    Name = "Caixa Econômica Federal",
                    Cnpj = "07526847000140",
                    WebPath = "https://www.caixa.gov.br"
                },
                new FinancialOrganization
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    BrandName = "XP Investimentos",
                    Name = "XP Investimentos Corretora de Valores S.A.",
                    Cnpj = "15326595000168",
                    WebPath = "https://www.xpinvestimentos.com.br"
                }
            };
        }

        /// <summary>
        /// Retorna uma organização financeira mockada pelo ID.
        /// </summary>
        /// <param name="id">ID da organização</param>
        /// <returns>Organização financeira ou null se não encontrada</returns>
        public static FinancialOrganization? GetMockFinancialOrganizationById(Guid id)
        {
            return GetMockFinancialOrganizations().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Retorna uma organização financeira mockada pelo nome da marca.
        /// </summary>
        /// <param name="brandName">Nome da marca</param>
        /// <returns>Organização financeira ou null se não encontrada</returns>
        public static FinancialOrganization? GetMockFinancialOrganizationByBrandName(string brandName)
        {
            return GetMockFinancialOrganizations().FirstOrDefault(x => x.BrandName.Equals(brandName, StringComparison.OrdinalIgnoreCase));
        }
    }
}
