using System.Data.Entity;
public interface IShopItemService
{
    Task<ShopItemModel> GetShopItem(Guid id);
    Task<List<ShopItemModel>> GetAllShopItems();
    Task CreateShopItem(ShopItemModel item);
    Task UpdateShopItem(Guid id, ShopItemModel item);
    Task RemoveShopItem(Guid id);
}

public class ShopItemService : IShopItemService
{
    private readonly AppDbContext _context;
    public ShopItemService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ShopItemModel> GetShopItem(Guid id)
    {
        var shopItem = _context.ShopItems.FirstOrDefaultAsync(x => x.Id == id);
        if (shopItem == null) throw new NullReferenceException("This Shopitem does not exist!");
        return await shopItem;
    }
    public async Task<List<ShopItemModel>> GetAllShopItems() => await _context.ShopItems.ToListAsync();

    public async Task CreateShopItem(ShopItemModel item)
    {
        if (item == null) throw new NullReferenceException(nameof(item));
        await _context.ShopItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShopItem(Guid id, ShopItemModel item)
    {
        ShopItemModel? ShopItem = await GetShopItem(id);
        ShopItem.Price = item.Price;
        ShopItem.Name = item.Name;
        ShopItem.Description = item.Description;
        await _context.SaveChangesAsync();
    }

    public async Task RemoveShopItem(Guid id)
    {
        ShopItemModel item = await GetShopItem(id);
        if (item == null) throw new NullReferenceException("This item does not exist!");
        _context.ShopItems.Remove(item);
        await _context.SaveChangesAsync();
    }
}