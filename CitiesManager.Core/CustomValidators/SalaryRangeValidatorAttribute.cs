using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CitiesManager.Core.CustomValidators
{
    public class SalaryRangeValidatorAttribute : ValidationAttribute
    {
        public string _otherProperty { get; set; }
        public SalaryRangeValidatorAttribute(string otherProperty) {
            _otherProperty = otherProperty;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value != null)
            {
                //Retrieve To expected salary value
                Decimal expected_salary = Convert.ToDecimal(value);

                //Retrieve current salary value using reflections
                //Model binding feature will bind the incoming http request parameters from various sources (Body, querystring, headers etc) to model class, and this model will be present as object
                //We can't access property values directly from this dynamic object, so that we are using reflections
                //Reflections are used to retrive the property values from object which is created dynamically asp.net model binders
                PropertyInfo? otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);

                if (otherPropertyInfo != null)
                {

                    Decimal current_salary = Convert.ToDecimal(otherPropertyInfo.GetValue(validationContext.ObjectInstance));
                    if (expected_salary < current_salary)
                    {
                        return new ValidationResult(ErrorMessage, new string[] { validationContext.MemberName });
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
            return null;
        }
    }
}
