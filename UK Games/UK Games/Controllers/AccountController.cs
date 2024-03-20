using Microsoft.AspNetCore.Mvc;

namespace UK_Games.Controllers;

public class AccountController : Controller
{
    // View account and profile
    public IActionResult Index()
    {
        return View();
    }
    
    // Login Form
    public IActionResult Login()
    {
        return View();
    }
    
    // Logout Action
    public IActionResult Logout()
    {
        return View();
    }
}