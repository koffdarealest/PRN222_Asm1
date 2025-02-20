using System;
using System.Collections.Generic;

namespace PRN222.Assignment1.Data.Models;

public class Attendee
{
    public int AttendeeId { get; set; }

    public int EventId { get; set; }

    public int? UserId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public DateTime? RegistrationTime { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User? User { get; set; }
}
