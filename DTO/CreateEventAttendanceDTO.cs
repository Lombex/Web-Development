public class CreateEventAttendanceDTO
{
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public int Rating { get; set; }
    public string Feedback { get; set; }
}
