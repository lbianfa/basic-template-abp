using BookStore.Books;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.CustomValidations
{
    public class BookTypeValidation : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult("Type is required");
            }

            var validTypes = Enum.GetNames(typeof(BookType));
            if (!validTypes.Contains(value.ToString()) ) 
            {
                return new ValidationResult($"Invalid Type. Valid types are: {string.Join(", ", validTypes)}");
            }

            return ValidationResult.Success;
        }
    }
}
