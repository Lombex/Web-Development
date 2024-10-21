using System;

public class EventAttendance
{
    public Guid Id { get; set; }
    public Guid UserID { get; set; }
    public Guid EventID { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }
}
