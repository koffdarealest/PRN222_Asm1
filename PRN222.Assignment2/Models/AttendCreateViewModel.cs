using System.ComponentModel.DataAnnotations;

namespace PRN222.Assignment2.Models
{
    public class AttendCreateViewModel
    {
        public int EventId { get; set; }

        public int? UserId { get; set; }

        [Required(ErrorMessage = "Please enter Name")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter Email")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        public DateTime? RegistrationTime { get; set; }

        public string? EventTitle { get; set; }
    }
}
