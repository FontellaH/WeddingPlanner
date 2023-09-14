#pragma warning disable CS8618 //#4
using System.ComponentModel.DataAnnotations;  //#4


namespace WeddingPlanner.Models;  //#5


public class LogUser //#6
{

    //Email
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string LogEmail { get; set; }


    //Password
    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    public string LogPassword { get; set; }

}