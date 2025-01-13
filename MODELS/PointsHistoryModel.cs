public class PointsHistory
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; }
    public DateTime Timestamp { get; set; }
}