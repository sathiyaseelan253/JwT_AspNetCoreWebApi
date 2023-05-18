using CitiesManager.Core.CustomValidators;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Person name can't be blank")]
        public string PersonName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage ="Email should be in proper email address format")]
        [Remote(action:"IsEmailAlreadyRegistered",controller:"Account",ErrorMessage ="Email is already in use")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage ="Phone number can't be blank")]
        [RegularExpression("^[0-9]{10}$",ErrorMessage ="Phone number should contain digits only")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password can't be blank")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Confirm Password can't be blank")]
        [Compare("Password",ErrorMessage ="Password and confirm password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
        //Custom validation - error message passed
        [MinimumYearValidation(2000,ErrorMessage ="Year should not be greater than {0}")]
        //Custom validation - error message Not Passed
        //[MinimumYearValidation(2000)]
        public DateTime DateOfBirth { get; set; }

        //Custom validations with multiple properties
        public decimal CurrentSalary { get; set; }


        [SalaryRangeValidator("CurrentSalary", ErrorMessage ="Expected salary should be greater than current salary")]
        public decimal ExpectedSalary { get; set; }

    }
}
