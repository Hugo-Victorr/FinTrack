using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OpenFinance.API.Services;
using OpenFinance.Model.Dtos;
using OpenFinance.Model.Entities;
using OpenFinance.Model.Enums;

namespace OpenFinance.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMapper _mapper;

        public AccountController(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém todas as contas.
        /// </summary>
        /// <returns>Lista de contas</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var mockAccounts = AccountMockService.GetMockAccounts();
            var accountDtos = _mapper.Map<List<AccountDto>>(mockAccounts);
            return Ok(accountDtos);
        }

        /// <summary>
        /// Obtém contas de um cliente específico pelo ID.
        /// </summary>
        /// <param name="customerId">ID do cliente</param>
        /// <returns>Lista de contas do cliente</returns>
        [HttpGet("{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetByCustomerId(Guid customerId)
        {
            var mockAccounts = AccountMockService.GetMockAccountsByCustomerId(customerId);

            if (!mockAccounts.Any())
            {
                return NotFound(new { message = "Nenhuma conta encontrada para este cliente." });
            }

            var accountDtos = _mapper.Map<List<AccountDto>>(mockAccounts);
            return Ok(accountDtos);
        }

        /// <summary>
        /// Obtém contas de um cliente específico pelo CPF.
        /// </summary>
        /// <param name="cpf">CPF do cliente</param>
        /// <returns>Lista de contas do cliente</returns>
        [HttpGet("cpf/{cpf}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAccountsByCpf(string cpf)
        {
            // Buscar cliente pelo CPF
            var costumer = CostumerMockService.GetMockCostumerByCpf(cpf);

            if (costumer == null)
            {
                return NotFound(new { message = $"Cliente com CPF '{cpf}' não encontrado." });
            }

            // Obter contas do cliente
            var accounts = AccountMockService.GetMockAccountsByCustomerId(costumer.Id);

            if (!accounts.Any())
            {
                return NotFound(new { message = $"Nenhuma conta encontrada para o cliente com CPF '{cpf}'." });
            }

            var accountDtos = _mapper.Map<List<AccountDto>>(accounts);
            return Ok(new { costumer = _mapper.Map<CostumerDto>(costumer), accounts = accountDtos });
        }

        /// <summary>
        /// Cria uma nova conta.
        /// </summary>
        /// <param name="accountDto">Dados da conta</param>
        /// <returns>Conta criada</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mock: mapear DTO para entidade
            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            account.CreatedAt = DateTime.Now.ToLocalTime();

            // Mock: retornar conta criada
            var createdAccountDto = _mapper.Map<AccountDto>(account);
            return CreatedAtAction(nameof(GetByCustomerId), new { customerId = account.CustomerId }, createdAccountDto);
        }

        /// <summary>
        /// Atualiza uma conta existente.
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <param name="accountDto">Dados atualizados da conta</param>
        /// <returns>Status da operação</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Update(Guid id, [FromBody] AccountDto accountDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Mock: verificar se conta existe
            if (id == Guid.Empty)
            {
                return NotFound(new { message = "Conta não encontrada." });
            }

            // Mock: atualizar conta
            accountDto.Id = id;

            return Ok(new { message = "Conta atualizada com sucesso.", data = accountDto });
        }

        /// <summary>
        /// Deleta uma conta existente.
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <returns>Status da operação</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(Guid id)
        {
            // Mock: verificar se conta existe
            if (id == Guid.Empty)
            {
                return NotFound(new { message = "Conta não encontrada." });
            }

            // Mock: deletar conta (soft delete)
            return NoContent();
        }
    }
}
