public class Badge
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int RequiredPoints { get; set; }
    public string ImageUrl { get; set; }
    public List<User> Users { get; set; } = new List<User>();
}