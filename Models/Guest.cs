#pragma warning disable CS8618 //#1

using System.ComponentModel.DataAnnotations;  //#2
using Microsoft.EntityFrameworkCore.Metadata.Internal; //#3

namespace WeddingPlanner.Models;  //#4

public class Guest //join table
{
    [Key]
    public int GuestId { get; set; }

    public int WeddingId { get; set; }  // // Foreign key for the Wedding the guest is attending

    public int UserId { get; set; }  // Foreign key for the User who RSVP'd
    public User? AttendingUser { get; set; }  // Nav  prop for  the User who RSVP'd

    public Wedding? WeddingList { get; set; } //// Nav prop of the Wedding the guest is attending

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;


}