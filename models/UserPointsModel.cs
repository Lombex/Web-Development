using System.Collections.Generic;

public record UserPointsModel
{
    public int AllTimePoints { get; set; } = 0;
    public int PointAmount { get; set; } = 0;
    public ICollection<ShopItemModel> Items { get; set; } = new List<ShopItemModel>();
}
