using System;

public class Attendance
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    // Relatie met EventAttendance
    public Guid EventAttendanceId { get; set; }
    public EventAttendance EventAttendance { get; set; }

    // Relatie met User
    public Guid UserId { get; set; }
    public User User { get; set; }
}

