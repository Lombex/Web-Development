using System;

public class User
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public UserRole Role { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RecuringDays { get; set; }
    public UserPointsModel Points { get; set; } = new UserPointsModel();
    public ICollection<EventAttendance> EventAttendances { get; set; } = new List<EventAttendance>();
}
