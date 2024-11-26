using System.Data.Entity;
public interface IShopItemService
{
    Task<ShopItems> GetShopItem(Guid id);
    Task CreateShopItem(ShopItems item);
    Task UpdateShopItem(Guid id, float Price, string Name, string Description);
    Task RemoveShopItem(Guid id);
}

public class ShopItemService : IShopItemService
{
    private readonly AppDbContext _context;
    public ShopItemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ShopItems> GetShopItem(Guid id)
    {
        var shopItem = _context.ShopItems.FirstOrDefaultAsync(x => x.Id == id);
        if (shopItem == null) throw new NullReferenceException("This Shopitem does not exist!");
        return await shopItem;
    }

    public async Task CreateShopItem(ShopItems item)
    {
        if (item == null) throw new NullReferenceException(nameof(item));
        await _context.ShopItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShopItem(Guid id, float Price, string Name, string Description)
    {
        ShopItems? ShopItem = await GetShopItem(id);
        ShopItem.Price = Price;
        ShopItem.Name = Name;
        ShopItem.Description = Description;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveShopItem(Guid id)
    {
        ShopItems item = await GetShopItem(id);
        if (item == null) throw new NullReferenceException("This item does not exist!");
        _context.ShopItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}