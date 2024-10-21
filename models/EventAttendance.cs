using System;

public class EventAttendance
{
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public Guid EventID { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }

     public EventAttendance( Guid userId, Guid eventId, int rating, string? feedback = null)
    {
        Id = Guid.NewGuid(); // Automatically generate a new ID
        UserID = userId;
        EventID = eventId;
        Rating = rating;
        Feedback = feedback;
    }
}
