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
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Czy zapamiętać Twoje hasło?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "* Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Hasło musi mieć minimum {2} znaków", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "* Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "* Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Nazwisko")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Display(Name = "Imię")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "* Data urodzenia")]
        [PastDate]
        public DateTime Birthday { get; set; }

        [Required]
        [Display(Name = "* Płeć")]
        public bool Gender { get; set; }

        [Display(Name = "Miasto lub miejscowość")]
        [MaxLength(50)]
        public string CityOrVillage { get; set; }

        [Display(Name = "Powiat")]
        [MaxLength(50)]
        public string District { get; set; }

        [Display(Name = "Ulica")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Display(Name = "Numer domu")]
        [MaxLength(10)]
        public string House { get; set; }

        [Display(Name = "Numer lokalu")]
        [MaxLength(10)]
        public string Flat { get; set; }

        [RegularExpression("([0-9]{2}-[0-9]{3})", ErrorMessage = "Kod pocztowy musi mieć postać 00-000")]
        [Display(Name = "Kod pocztowy")]
        [MaxLength(6)]
        public string PostalCode { get; set; }
    }

    public class RegisterDoctorViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "* Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "* Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "* Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Wprowadzone hasła różnią się od siebie")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "* Specialność")]
        public string Speciality { get; set; }

        [Display(Name = "Nazwisko")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Display(Name = "Imię")]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Miasto lub miejscowość")]
        [MaxLength(50)]
        public string CityOrVillage { get; set; }

        [Display(Name = "Powiat")]
        [MaxLength(50)]
        public string District { get; set; }

        [Display(Name = "Ulica")]
        [MaxLength(50)]
        public string Street { get; set; }

        [Display(Name = "Numer domu")]
        [MaxLength(10)]
        public string House { get; set; }

        [Display(Name = "Numer lokalu")]
        [MaxLength(10)]
        public string Flat { get; set; }

        [Display(Name = "Kod pocztowy")]
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
