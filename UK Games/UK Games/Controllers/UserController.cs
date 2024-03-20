using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UK_Games.Models;

namespace UK_Games.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;
    // private List<User> users = new List<User>()
    // {
    //     { username: "Joe", password: "123" },
    //     { username: "Alice", password: "abc" }
    // };
    // private Dictionary<string,User> users = new Dictionary<string,User>()
    // {
    //     { "Joe", new User("Joe", "123") },
    //     { "Alice", new User("Alice", "abc") }
    // };
    // private User? loggedin = null;

    public UserController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    // public IActionResult Login(string? username, string? password)
    // {
    //     if (users.ContainsKey(username ?? "") && users[username ?? ""]?.Password == (password ?? ""))
    //         return RedirectToAction("Profile");
    //     return RedirectToAction("Login");
    // }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}