using System;

public class EventAttendance
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }
    public bool HasAttended { get; set; }
    public DateTime? CheckInTime { get; set; }
    public bool FeedbackProvided { get; set; }
    public int EarnedPoints { get; set; }
}
