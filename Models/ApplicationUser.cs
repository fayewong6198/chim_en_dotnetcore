using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace Chim_En_DOTNET.Models
{
  public class ApplicationUser : IdentityUser
  {

    public RoleChoices Role { get; set; }
    public string Mobile { get; set; }
    public string FullName { get; set; }

    public bool IsStaff { get; set; }

    public string EmailToken { get; set; }

    public bool IsSuperUser { get; set; }
    public GenderChoices? Gender { get; set; }

    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public enum RoleChoices : int
    {
      User = 0,
      Staff = 1,
      Admin = 2
    }

    public ApplicationUser()
    {
      this.Gender = GenderChoices.Male;
      this.Role = RoleChoices.User;
      this.CreatedAt = DateTime.Now;
    }

    public enum GenderChoices
    {
      Male, Female
    }
  }
}