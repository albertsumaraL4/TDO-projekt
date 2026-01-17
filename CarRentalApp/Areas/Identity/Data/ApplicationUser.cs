using CarRentalApp.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CarRentalApp.Areas.Identity.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    //public int Id { get; set; }

    [Required, StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required, StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; }

    public bool IsAdmin { get; set; } = false;

    public ICollection<Reservation> Reservations { get; set; }
}

