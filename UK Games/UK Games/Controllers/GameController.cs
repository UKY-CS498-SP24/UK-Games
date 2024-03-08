using Microsoft.AspNetCore.Mvc;

namespace UK_Games.Controllers;

public class GameController : Controller
{
    // GOING TO A GAME PAGE
    public IActionResult Index()
    {
        return View();
    }
}