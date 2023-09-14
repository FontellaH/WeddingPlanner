#pragma warning disable CS8618 //#4


using System.ComponentModel.DataAnnotations;  //#4
using System.ComponentModel.DataAnnotations.Schema; //#8


namespace WeddingPlanner.Models;

public class User
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "First Name is required.")]
    [MinLength(2, ErrorMessage = "First Name must be at least 2 characters.")]
    public string FirstName { get; set; }


    //Last Name
    [Required(ErrorMessage = "Last Name is required.")]
    [MinLength(2, ErrorMessage = "Last Name must be at least 2 characters.")]
    public string LastName { get; set; }


    //Email
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    [UniqueEmail]
    public string Email { get; set; }


    //Password
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string Password { get; set; }



    //Crrated/Updated
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


    //Password Confirmation  //#7
    [NotMapped]  /// lets them know that they dont want this in the data form
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password Confirmation must match.")]  // There is also a built-in attribute for comparing two fields we can use
    public string ConfirmPassword { get; set; }
}



public class UniqueEmailAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Though we have Required as a validation, sometimes we make it here anyways
        // In which case we must first verify the value is not null before we proceed
        if (value == null)
        {
            // If it was, return the required error
            return new ValidationResult("Email is required!");
        }

        // This will connect us to our database since we are not in our Controller
        MyContext _context = (MyContext)validationContext.GetService(typeof(MyContext));
        // Check to see if there are any records of this email in our database
        if (_context.Users.Any(e => e.Email == value.ToString()))
        {
            // If yes, throw an error
            return new ValidationResult("Email must be unique!");
        }
        else
        {
            // If no, proceed
            return ValidationResult.Success;
        }
    }
}

//#9 Make  new File in Models called MyContext.cs