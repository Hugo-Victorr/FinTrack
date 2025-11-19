namespace OpenFinance.Model.Dtos
{
    /// <summary>
    /// DTO para organização financeira.
    /// </summary>
    public class FinancialOrganizationDto
    {
        public Guid Id { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string WebPath { get; set; }
    }
}
