using PRN222.Assignment1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment1.Business.DTOs
{
    public class EventDto
    {
        public int EventId { get; set; }

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string? Location { get; set; }

        public int CategoryId { get; set; }

        public virtual ICollection<Attendee> Attendees { get; set; }

        public virtual EventCategory Category { get; set; }
    }
}
