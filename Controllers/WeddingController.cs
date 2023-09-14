using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;  //#1


[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<UserController> _logger;
    private MyContext _context;

    public WeddingController(ILogger<UserController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }



    //Main Page
    [HttpGet("weddings")]
    public IActionResult Dashboard()
    {
        List<Wedding> allWeddings = _context.Weddings.Include(w => w.GuestList).ToList();  //// Include the related Wedding_Of entity

        return View(allWeddings);
    }

    [HttpGet("weddings/new")]
    public ViewResult NewWedding()
    {
        return View();
    }


    //Create Wedding
    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            return View("NewWedding", newWedding);
        }
        int? userId = HttpContext.Session.GetInt32("UUID");

        if (userId.HasValue)
        {
            newWedding.UserId = userId.Value;
            _context.Add(newWedding);
            _context.SaveChanges();
        }
        return ViewWedding(newWedding.WeddingId);  //create route to view one page
    }


//Delete Action for my Dashboard page

[HttpPost("weddings/{id}/delete")]
public RedirectToActionResult DeleteWedding(int id)
{
    Wedding toDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == id);

    if (toDelete != null)
    {
        _context.Weddings.Remove(toDelete);
        _context.SaveChanges();
    }

    return RedirectToAction("Dashboard");
}





//*****Need..Work********
[HttpPost("weddings/{id}/guest")]
public RedirectToActionResult ToggleGuest(int id)
{
    //gets the Id og the wedding to toggle a guest, gets the uuid user idfrom session and if its not found it will be null
    int UUID = (int)HttpContext.Session.GetInt32("UUID");

    //checking the wl  to see if the guest exist with both guestid and userid with the uuid
    Guest existingGuest = _context.Guests.FirstOrDefault(g => g.WeddingId == id && g.UserId == UUID);
    
    if (existingGuest == null) // user didnt rsvp
    {
        Guest newGuest = new()  // this is create a new guest if they didnt rsvp
        {
            UserId = UUID,
            WeddingId = id
        };
        _context.Add(newGuest); //adds them to the db
    }
    else
    {
        _context.Remove(existingGuest);       
    }
    _context.SaveChanges();

    return RedirectToAction("Dashboard"); //after done being toggled it sends them to Dashboard to see update
}




//ViewOne route
[HttpGet("weddings/{id}/view")]
public IActionResult ViewWedding(int id)
{
    Wedding? oneWedding =_context.Weddings  //Looking in the db for a certain wedding w/ the id
                                    .Include(w=>w.GuestList) //access the guest list associated with the wedding.                                   
                                    .ThenInclude(ug=>ug.AttendingUser) //get teh info about the guest related to the wedding
                                    .FirstOrDefault(w=>w.WeddingId == id); //Looking for the 1st wedding match in the db
    if (oneWedding == null)
    {
        return RedirectToAction("Dashboard");
    }  
    return View("ViewWedding", oneWedding);                              
} 




    // Name this anything you want with the word "Attribute" at the end
    public class SessionCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Find the session, but remember it may be null so we need int?
            int? userId = context.HttpContext.Session.GetInt32("UUID");
            // Check to see if we got back null
            if (userId == null)
            {
                // Redirect to the Index page if there was nothing in session
                // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }





    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
