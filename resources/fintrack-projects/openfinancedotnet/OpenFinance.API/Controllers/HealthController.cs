using Microsoft.AspNetCore.Mvc;

namespace OpenFinance.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Verifica o status de saúde da API.
        /// </summary>
        /// <returns>Mensagem indicando que a API está funcionando.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetHealthStatus()
        {
            return Ok(new { status = "API is running smoothly." });
        }
    }
}
