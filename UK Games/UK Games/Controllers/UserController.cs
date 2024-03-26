using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UK_Games.Infrastructure;
using UK_Games.Models;

namespace UK_Games.Controllers;

public class UserController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public UserController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(IFormCollection fc)
    {
        try
        {
            string first = fc["FirstName"];
            string last = fc["LastName"];
            DateTime dob = DateTime.Parse(fc["DOB"]);
            string username = fc["Username"];
            string email = fc["Email"];
            string password = fc["Password"];
            string confirmPassword = fc["ConfirmPassword"];

            if (first == null ||
                last == null ||
                dob == null ||
                username == null ||
                email == null ||
                password == null ||
                confirmPassword == null)
            {
                ModelState.AddModelError("", "Unknown error occured, please try again...");
                return View();
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match, please try again.");
                return View();
            }

            foreach (User u in DataUtil.Data.GetUsers())
            {
                if (u.Username.ToUpper() == username.ToUpper())
                {
                    ModelState.AddModelError("", "Username already exists, please pick another one.");
                    return View();
                }

                if (u.Email.ToUpper() == email.ToUpper())
                {
                    ModelState.AddModelError("", "Email already registered... do you need to login?");
                    return View();
                }
            }

            User user = new User(username, first, last, dob, email, password);

        }
        catch (Exception e)
        {
            GeneralMethods.HandleException(e);
            ModelState.AddModelError("", "Please try again! If this issue still occurs, contact us at info@ukgames.net for further assistance.");
            return View();
        }
        
        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(IFormCollection fc)
    {
        try
        {
            string username = fc["Username"];
            string password = fc["Password"];
            
            if (username == null || password == null) // didn't fill out a field, shouldn't happen
            {
                ModelState.AddModelError("", "Unknown error occured, please try again...");
                return View();
            }

            Console.WriteLine("[ LOGIN ATTEMPT ] Checking login information...");
            
            Dictionary<bool, User> checkLoginInfo = GeneralMethods.ConfirmUser(username, password, HttpContext.Session);
            
            Console.WriteLine("[ LOGIN ATTEMPT ] Received information...");

            if (checkLoginInfo.Keys.ToList()[0]) // username + password is valid
            {
                return RedirectToAction("Index", "Home");
            }

            //else return to login
            ModelState.AddModelError("", "Incorrect username or password...");
            return View();
        }
        catch (Exception e)
        {
            GeneralMethods.HandleException(e);
            ModelState.AddModelError("", "Incorrect username or password...");
            return View();
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}