using System.Collections.Generic;

public record UserPointsModel
{
    public int AllTimePoints { get; set; } = 0;
    public int PointAmount { get; set; } = 0;
    public List<ShopItems> Items { get; set; } = new List<ShopItems>();
    public int CurrentStreak { get; set; } = 0;
    public DateTime LastPointsEarned { get; set; }
}