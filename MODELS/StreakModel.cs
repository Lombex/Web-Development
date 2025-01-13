public class Streak
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int CurrentStreak { get; set; }
    public int HighestStreak { get; set; }
    public DateTime LastAttendance { get; set; }
}