using FinTrack.Model.Entities;
using System.Text.Json.Serialization;

namespace FinTrack.Model.DTO
{
    public class OpenFinanceCustomerAccountsDto
    {
        [JsonPropertyName("costumer")]
        public Costumer Costumer { get; set; }

        [JsonPropertyName("accounts")]
        public List<Account> Accounts { get; set; } = new();
    }
}
