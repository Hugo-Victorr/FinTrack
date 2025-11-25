using FinTrack.Model.Enums;

namespace FinTrack.Model.Entities
{
    public class Costumer : BaseEntity
    {
        /// <summary>
        /// Número do Cadastro de Pessoas Físicas (CPF) do cliente.
        /// Exemplo: 12345678900
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Nome completo do cliente.
        /// Exemplo: Maria da Silva Santos
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Endereço de email do cliente.
        /// Exemplo: maria.silva@example.com
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Data de nascimento do cliente.
        /// Exemplo: 1985-06-25
        /// </summary>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Estado civil do cliente.
        /// Exemplos: SINGLE (Solteiro), MARRIED (Casado), DIVORCED (Divorciado), WIDOWED (Viúvo)
        /// </summary>
        public MaritalStatusEnum MaritalStatus { get; set; }

        /// <summary>
        /// Gênero ou sexo do cliente.
        /// Exemplos: FEMALE (Feminino), MALE (Masculino)
        /// </summary>
        public GenderEnum Gender { get; set; }
    }
}
