using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WebDevMidTermProject.Models;

namespace WebDevMidTermProject.Custom_Validations
{
    public class ValidateWSCheckbox : ValidationAttribute
    {
        public ValidateWSCheckbox()
        {
            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && (bool)value == true)
            {
                if(validationContext.ObjectInstance != null)
                {
                    CodeModel codeModel = (CodeModel)validationContext.ObjectInstance;
                    if(codeModel != null && !String.IsNullOrWhiteSpace(codeModel.InputSecretMessage))
                    {
                        //Regex RgxUrl = new Regex("[^a-z0-9]");
                        //bool blnContainsSpecialCharacters = RgxUrl.IsMatch(codeModel.InputSecretMessage);
                        if (!codeModel.InputSecretMessage.Contains(" "))
                        {
                            return new ValidationResult("Please Enter a Valid String. " +
                                "The String entered above does not contain whitespaces or else untick the checkbox");
                        }
                        
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
