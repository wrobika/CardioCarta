using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardioCarta.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [PastDate]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Required]
        [Display(Name = "City or village")]
        [MaxLength(50)]
        public string CityOrVillage { get; set; }

        [Required]
        [Display(Name = "District")]
        [MaxLength(50)]
        public string District { get; set; }

        [Display(Name = "Street")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "House Number")]
        [MaxLength(10)]
        public string House { get; set; }

        [Display(Name = "Flat Number")]
        [MaxLength(10)]
        public string Flat { get; set; }

        [Required]
        [RegularExpression("([0-9]{2}-[0-9]{3})", ErrorMessage = "Kod pocztowy musi mieć postać 00-000")]
        [MaxLength(6)]
        public string PostalCode { get; set; }
    }

    public class RegisterDoctorViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Speciality")]
        public string Speciality { get; set; }

        [Required]
        [Display(Name = "Surname")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "First name")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        [PastDate]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public bool Gender { get; set; }

        [Required]
        [Display(Name = "City or village")]
        [MaxLength(50)]
        public string CityOrVillage { get; set; }

        [Required]
        [Display(Name = "District")]
        [MaxLength(50)]
        public string District { get; set; }

        [Display(Name = "Street")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Required]
        [Display(Name = "House Number")]
        [MaxLength(10)]
        public string House { get; set; }

        [Display(Name = "Flat Number")]
        [MaxLength(10)]
        public string Flat { get; set; }

        [Required]
        [Display(Name = "Postal code")]
        [RegularExpression("([0-9]{2}-[0-9]{3})", ErrorMessage = "Kod pocztowy musi mieć postać 00-000")]
        [MaxLength(6)]
        public string PostalCode { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class PastDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && (DateTime)value < DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date of birth must be past");
            }
        }
    }
}
