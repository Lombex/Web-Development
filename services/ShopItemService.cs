using System.Data.Entity;
public interface IShopItemService
{
    Task<ShopItems> GetShopItem(Guid id);
    Task<IEnumerable<ShopItems>> GetAllShopItems();
    Task CreateShopItem(ShopItems item);
    Task UpdateShopItem(Guid id, ShopItems item);
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

    public async Task<IEnumerable<ShopItems>> GetAllShopItems() => await _context.ShopItems.ToListAsync();

    public async Task CreateShopItem(ShopItems item)
    {
        if (item == null) throw new NullReferenceException(nameof(item));
        await _context.ShopItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShopItem(Guid id, ShopItems item)
    {
        ShopItems? ShopItem = await GetShopItem(id);
        ShopItem.Price = item.Price;
        ShopItem.Name = item.Name;
        ShopItem.Description = item.Description;
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