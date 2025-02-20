using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment1.Business.DTOs
{
    public class AttendeeDto
    {
        public int AttendeeId { get; set; }

        public int EventId { get; set; }

        public int? UserId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public DateTime? RegistrationTime { get; set; }
    }
}
