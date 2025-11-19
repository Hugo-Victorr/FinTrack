using OpenFinance.Model.Enums;

namespace OpenFinance.Model.Dtos
{
    /// <summary>
    /// DTO para cliente.
    /// </summary>
    public class CostumerDto
    {
        public Guid Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public MaritalStatusEnum MaritalStatus { get; set; }
        public GenderEnum Gender { get; set; }
    }
}
