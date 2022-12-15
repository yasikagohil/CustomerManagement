using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerXtreme
{
    public class ValidationAge : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                DateTime todayDate = DateTime.Now;

                DateTime givenAge = (DateTime)value;
                int age = todayDate.Year - givenAge.Year;
                if (todayDate.Month < givenAge.Month || (todayDate.Month == givenAge.Month && todayDate.Day < givenAge.Day))
                {
                    age--;
                }
                //return age;
                if (age < 18)
                {
                    return new ValidationResult("The age must be greater than 18");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }
    }
}