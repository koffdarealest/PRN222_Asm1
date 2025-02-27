using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment2.Models.Validations
{
    public class EndTimeAfterStartTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var eventCreateViewModel = (EventCreateViewModel)validationContext.ObjectInstance;

            if (eventCreateViewModel.StartTime.Value >= eventCreateViewModel.EndTime.Value)
            {
                return new ValidationResult("End Time must be after Start Time.");
            }
            return ValidationResult.Success;
        }
    }
}
