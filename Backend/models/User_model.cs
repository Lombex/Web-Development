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
    public UserPoints_model Points { get; set; } = new UserPoints_model();
    public ICollection<EventAttendance_model> EventAttendances { get; set; } = new List<EventAttendance_model>();
}
