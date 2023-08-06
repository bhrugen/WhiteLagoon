using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WhiteLagoon.Web.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name="Phone Number")]
        public string? PhoneNumber { get; set; }

        public string? RedirectUrl { get; set; }
        public string? Role { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? RoleList { get; set; }
    }
}
