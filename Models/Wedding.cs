#pragma warning disable CS8618 //#1
using System.ComponentModel.DataAnnotations;  //#2
using System.ComponentModel.DataAnnotations.Schema; //#3


namespace WeddingPlanner.Models;

public class Wedding
{
    [Key]
    public int WeddingId { get; set; }

    [Required(ErrorMessage = "Bride name is required.")]
    public string BrideName { get; set; }

    [Required(ErrorMessage = "Groom name is required.")]
    public string GroomName { get; set; }

    [Required(ErrorMessage = " Wedding date is required.")]
    [FutureDate(ErrorMessage = "The wedding date must be in the future.")]
    public DateTime WeddingDate { get; set; }

    [Required(ErrorMessage = "Address is required.")]
    public string WeddingAddress { get; set; }

    public int UserId { get; set; }
    public User? Creater {get; set;}

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public List<Guest> GuestList { get; set; } = new(); //nav prop for guest this is joining them... 

}

// Custom validation attribute for checking future dates
public class FutureDateAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime date = (DateTime)value;
        DateTime now = DateTime.Now;

        if (date <= now)
        {
            return new ValidationResult("The wedding date must be in the future.");
        }

        return ValidationResult.Success;
    }
}





