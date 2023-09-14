using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;

using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;  //#1


// [SessionCheck]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context;

    public UserController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }



    //Main Page
    [HttpGet("")]
    public IActionResult Index()
    {
        return View();
    }



    //Register Route
    [HttpPost("users/register")]
    public IActionResult RegisterUser(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        PasswordHasher<User> hasher = new();
        newUser.Password = hasher.HashPassword(newUser, newUser.Password);
        _context.Add(newUser);
        _context.SaveChanges();


        HttpContext.Session.SetInt32("UUID", newUser.UserId); //adding it into session
        HttpContext.Session.SetString("LoggedUser", newUser.FirstName);  //this logs the user to the top of each page when logged in
        return RedirectToAction("Dashboard", "Wedding");  //dont have a all post update once we have another controller
    }




    //Login Route
    [HttpPost("user/login")]
    public IActionResult LoginUser(LogUser logAttempt)
    {
        if (!ModelState.IsValid)
        {
            return View("Index");
        }
        User? dbUser = _context.Users.FirstOrDefault(u => u.Email == logAttempt.LogEmail);
        if (dbUser == null)
        {
            ModelState.AddModelError("LogPassword", "Invalid Information");
            return View("Index");
        }
        PasswordHasher<LogUser> hasher = new();
        PasswordVerificationResult pwCompareResult = hasher.VerifyHashedPassword(logAttempt, dbUser.Password, logAttempt.LogPassword);
        if (pwCompareResult == 0)
        {
            ModelState.AddModelError("LogPassword", "Invalid Information");
            return View("Index");
        }
        HttpContext.Session.SetInt32("UUID", dbUser.UserId); //adding it into session
        HttpContext.Session.SetString("LoggedUser", dbUser.FirstName);  //this logs the user to the top of each page when logged in
        return RedirectToAction("Dashboard", "Wedding");  //dont have a all post
    }


    //Logout Route
    [HttpPost("user/logout")]
    public IActionResult LogOut()
    {
        // Clear the user's session (e.g., remove user-specific data)
        HttpContext.Session.Clear(); //clearing the user session

        // Redirect the user to the login/register page
        return RedirectToAction("Index");  //return to index page
    }





    // Name this anything you want with the word "Attribute" at the end
    // public class SessionCheckAttribute : ActionFilterAttribute
    // {
    //     public override void OnActionExecuting(ActionExecutingContext context)
    //     {
    //         // Find the session, but remember it may be null so we need int?
    //         int? userId = context.HttpContext.Session.GetInt32("UUID");
    //         // Check to see if we got back null
    //         if (userId == null)
    //         {
    //             // Redirect to the Index page if there was nothing in session
    //             // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
    //             context.Result = new RedirectToActionResult("Index", "Home", null);
    //         }
    //     }
    // }






    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
