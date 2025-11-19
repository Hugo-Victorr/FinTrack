using OpenFinance.Model.Enums;

namespace OpenFinance.Model.Dtos
{
    /// <summary>
    /// DTO para conta.
    /// </summary>
    public class AccountDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FinancialOrganizationId { get; set; }
        public string FinancialOrganizationName { get; set; }
        public CurrencyTypeEnum Currency { get; set; }
        public AccountTypeEnum Type { get; set; }
        public string BranchCode { get; set; }
        public string Number { get; set; }
        public AccountStatusEnum Status { get; set; }
        public DateTime OpeningDate { get; set; }
    }
}
