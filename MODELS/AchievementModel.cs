public class Achievement
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PointsRequired { get; set; }
    public List<User> Users { get; set; } = new List<User>();
}