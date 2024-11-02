using System;
using System.ComponentModel.DataAnnotations;

public class MinimumAgeAttribute : ValidationAttribute
{
    private readonly int _minAge;

    public MinimumAgeAttribute(int minAge)
    {
        _minAge = minAge;
        ErrorMessage = $"Age must be at least {_minAge} years.";
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult("Date of Birth is required.");
        }

        if (value is DateTime dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;

            // If the birthday hasn't occurred yet this year, subtract one from the age.
            if (dob.Date > today.AddYears(-age)) age--;

            if (age >= _minAge)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(ErrorMessage);
            }
        }

        return new ValidationResult("Invalid date format.");
    }
}
