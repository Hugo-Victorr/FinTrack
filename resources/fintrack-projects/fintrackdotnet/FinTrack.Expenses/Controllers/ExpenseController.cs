using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Expenses.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExpenseController : ControllerBase
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}
