using PRN222.Assignment1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN222.Assignment1.Business.DTOs
{
    public class EventCategoryDto
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = null!;

        public virtual ICollection<Event> Events { get; set; }
    }
}
