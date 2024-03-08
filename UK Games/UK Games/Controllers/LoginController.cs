using Microsoft.AspNetCore.Mvc;

namespace UK_Games.Controllers;

public class LoginController : Controller
{
    // LOGGING IN / LOGGING OUT
    public IActionResult Index()
    {
        return View();
    }
}