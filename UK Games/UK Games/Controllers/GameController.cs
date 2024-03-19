using Microsoft.AspNetCore.Mvc;

namespace UK_Games.Controllers;

public class GameController : Controller
{
    // GOING TO A GAME PAGE
    
    /*
     *      localhost:3306/game/25
     */
    public IActionResult Index(int id)
    {
        ViewBag.GameID = id;
        return View();
    }
}