using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebDevMidTermProject.Models;

namespace WebDevMidTermProject.Custom_Validations
{
    public class ValidateSpecialCharCheckbox : ValidationAttribute
    {
        public ValidateSpecialCharCheckbox()
        {

        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && (bool)value == true)
            {
                if (validationContext.ObjectInstance != null)
                {
                    CodeModel codeModel = (CodeModel)validationContext.ObjectInstance;
                    if (codeModel != null && !String.IsNullOrWhiteSpace(codeModel.InputSecretMessage))
                    {
                        //Regex RgxUrl = new Regex("[^a-z0-9]");
                        Regex RgxUrl = new Regex("[^a-zA-Z0-9]");
                        bool blnContainsSpecialCharacters = RgxUrl.IsMatch(codeModel.InputSecretMessage);
                        if (!blnContainsSpecialCharacters)
                        {
                            return new ValidationResult("Please Enter a Valid String. " +
                                "The String entered above does not contain special characters or else untick the checkbox");
                        }
                        
                    }
                }
            }
            else if (value != null && (bool)value == false)
            {
                if (validationContext.ObjectInstance != null)
                {
                    CodeModel codeModel = (CodeModel)validationContext.ObjectInstance;
                    if (codeModel != null && !String.IsNullOrWhiteSpace(codeModel.InputSecretMessage))
                    {
                        Regex RgxUrl = new Regex("[^a-zA-Z0-9]");
                        bool blnContainsSpecialCharacters = RgxUrl.IsMatch(codeModel.InputSecretMessage);
                        if (blnContainsSpecialCharacters)
                        {
                            return new ValidationResult("Please check the Special Characters checkbox");
                        }

                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
