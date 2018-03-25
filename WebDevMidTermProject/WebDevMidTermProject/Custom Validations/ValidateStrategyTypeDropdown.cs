using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebDevMidTermProject.Models;

namespace WebDevMidTermProject.Custom_Validations
{
    public class ValidateStrategyTypeDropdown : ValidationAttribute
    {
        public ValidateStrategyTypeDropdown()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Please select a value from the dropdown");
            }

            return ValidationResult.Success;
        }
    }
}
