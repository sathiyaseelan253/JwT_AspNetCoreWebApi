using System.ComponentModel.DataAnnotations;

namespace CitiesManager.Core.CustomValidators
{
    /// <summary>
    /// Custom validators to validate & accept only year less than specified year
    /// </summary>
    public class MinimumYearValidationAttribute : ValidationAttribute
    {
        public int minYear { get; set; }
        public string defaultErrorMessage = "Please enter valid year less than {0}";
        public MinimumYearValidationAttribute()
        {

        }
        public MinimumYearValidationAttribute(int minimumYear)
        {
            minYear = minimumYear;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                DateTime date = (DateTime)value;
                if(date.Year > minYear)
                {
                    return new ValidationResult(string.Format(ErrorMessage??defaultErrorMessage,minYear));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
