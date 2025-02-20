using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment1.Models.Validations
{
    public class StartTimeBeforeNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var eventCreateViewModel = (EventCreateViewModel)validationContext.ObjectInstance;

            if (eventCreateViewModel.StartTime.Value <= DateTime.Now)
            {
                return new ValidationResult("Start Time must be after now.");
            }
            return ValidationResult.Success;
        }
    }
}
