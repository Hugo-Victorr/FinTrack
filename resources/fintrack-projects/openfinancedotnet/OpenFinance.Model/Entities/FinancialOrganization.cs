namespace OpenFinance.Model.Entities
{
    public class FinancialOrganization : BaseEntity
    {
        public string BrandName { get; set; } = "";
        public string Name { get; set; } = "";
        public string Cnpj { get; set; } = "";
        public string WebPath { get; set; } = "";
    }
}
