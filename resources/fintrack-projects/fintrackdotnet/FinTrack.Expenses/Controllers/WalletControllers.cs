using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Expenses.Controllers
{
    public class WalletControllers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
