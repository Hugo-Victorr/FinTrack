using OpenFinance.Model.Entities;
using OpenFinance.Model.Enums;

namespace OpenFinance.API.Services
{
    /// <summary>
    /// Serviço de mock para dados de clientes.
    /// </summary>
    public static class CostumerMockService
    {
        /// <summary>
        /// Retorna uma lista mockada de clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        public static List<Costumer> GetMockCostumers()
        {
            return new List<Costumer>
            {
                new Costumer
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Cpf = "12345678901",
                    Name = "Fabio Cabrini",
                    Email = "fabio.cabrini@example.com",
                    BirthDate = new DateTime(1985, 3, 15),
                    MaritalStatus = MaritalStatusEnum.MARRIED,
                    Gender = GenderEnum.MALE,
                    CreatedAt = DateTime.Now.ToLocalTime()
                },
                new Costumer
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Cpf = "98765432109",
                    Name = "Gabriel Lara",
                    Email = "gabriel.lara@example.com",
                    BirthDate = new DateTime(1990, 7, 22),
                    MaritalStatus = MaritalStatusEnum.SINGLE,
                    Gender = GenderEnum.MALE,
                    CreatedAt = DateTime.Now.ToLocalTime()
                },
                new Costumer
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Cpf = "55544433322",
                    Name = "Leide Aparecida",
                    Email = "leide.aparecida@example.com",
                    BirthDate = new DateTime(1978, 11, 8),
                    MaritalStatus = MaritalStatusEnum.MARRIED,
                    Gender = GenderEnum.FEMALE,
                    CreatedAt = DateTime.Now.ToLocalTime()
                },
                new Costumer
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Cpf = "11122233344",
                    Name = "Hugo Victor Lima",
                    Email = "hugo.victor@example.com",
                    BirthDate = new DateTime(2000, 2, 12),
                    MaritalStatus = MaritalStatusEnum.SINGLE,
                    Gender = GenderEnum.MALE,
                    CreatedAt = DateTime.Now.ToLocalTime()
                }
            };
        }

        /// <summary>
        /// Retorna um cliente mockado pelo ID.
        /// </summary>
        /// <param name="id">ID do cliente</param>
        /// <returns>Cliente ou null se não encontrado</returns>
        public static Costumer? GetMockCostumerById(Guid id)
        {
            return GetMockCostumers().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Retorna um cliente mockado pelo nome.
        /// </summary>
        /// <param name="name">Nome do cliente</param>
        /// <returns>Cliente ou null se não encontrado</returns>
        public static Costumer? GetMockCostumerByName(string name)
        {
            return GetMockCostumers().FirstOrDefault(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retorna um cliente mockado pelo CPF.
        /// </summary>
        /// <param name="cpf">CPF do cliente</param>
        /// <returns>Cliente ou null se não encontrado</returns>
        public static Costumer? GetMockCostumerByCpf(string cpf)
        {
            return GetMockCostumers().FirstOrDefault(x => x.Cpf.Equals(cpf, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retorna um cliente mockado pelo email.
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <returns>Cliente ou null se não encontrado</returns>
        public static Costumer? GetMockCostumerByEmail(string email)
        {
            return GetMockCostumers().FirstOrDefault(x => x.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }
    }
}
