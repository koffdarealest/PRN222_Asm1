using System;
using System.Collections.Generic;

namespace PRN222.Assignment1.Data.Models;

public class EventCategory
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();
}
