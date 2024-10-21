using System;

public class EventAttendance
{
    public Guid Id { get; set; }
    
    // Foreign key naar User
    public Guid UserId { get; set; }
    public User User { get; set; } // Navigatie-eigenschap

    // Foreign key naar Event
    public Guid EventId { get; set; }
    public Event Event { get; set; } // Navigatie-eigenschap

    public int Rating { get; set; }
    public string Feedback { get; set; }
}
