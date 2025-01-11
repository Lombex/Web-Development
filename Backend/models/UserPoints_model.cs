using System.Collections.Generic;

public record UserPoints_model
{
    public int AllTimePoints { get; set; } = 0;
    public int PointAmount { get; set; } = 0;
    public List<ShopItems> Items { get; set; } = new List<ShopItems>();
}
