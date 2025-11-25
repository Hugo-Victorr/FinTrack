namespace FinTrack.Model.Entities
{
    public class Account : BaseEntity
    {
        /// <summary>
        /// CHAVE ESTRANGEIRA (Foreign Key). ID do cliente a quem a conta pertence, estabelecendo o relacionamento 1:N.
        /// Exemplo: 999e8800-x00z-4x8k-a111-112233445566
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Instituição finaneira onde a conta está registrada.
        /// </summary>
        public FinancialOrganization Financial { get; set; }

        /// <summary>
        /// Código da moeda referente aos valores monetários da conta, seguindo o padrão ISO 4217.
        /// Exemplo: BRL
        /// </summary>
        public CurrencyTypeEnum Currency { get; set; }

        /// <summary>
        /// Tipo de conta. Reflete a finalidade da conta.
        /// Exemplos: CACC (Conta Corrente), SVGS (Poupança), SLRY (Conta Salário), CCRD (Cartão de Crédito)
        /// </summary>
        public AccountTypeEnum Type { get; set; }

        /// <summary>
        /// Código da agência sem o dígito verificador.
        /// Exemplo: 0001
        /// </summary>
        public string BranchCode { get; set; }

        /// <summary>
        /// Número completo da conta com o dígito verificador (sem separador).
        /// Exemplo: 1234567890
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Situação atual da conta.
        /// Exemplos: AVAILABLE (Ativa), BLOCKED (Bloqueada)
        /// </summary>
        public AccountStatusEnum Status { get; set; }

        /// <summary>
        /// Data de abertura da conta.
        /// Exemplo: 2018-01-15
        /// </summary>
        public DateTime OpeningDate { get; set; }

        /// <summary>
        /// Quantia atual depositada nesta conta na moeda indicada.
        /// Exemplo: 1523.75
        /// </summary>
        public decimal Amount { get; set; }
    }
}
