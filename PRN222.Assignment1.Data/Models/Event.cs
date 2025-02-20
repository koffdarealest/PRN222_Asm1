﻿using System;
using System.Collections.Generic;

namespace PRN222.Assignment1.Data.Models;

public class Event
{
    public int EventId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Location { get; set; }

    public int CategoryId { get; set; }

    public virtual ICollection<Attendee> Attendees { get; set; } = new List<Attendee>();

    public virtual EventCategory Category { get; set; } = null!;
}
