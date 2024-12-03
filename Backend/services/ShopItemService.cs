using Microsoft.EntityFrameworkCore;
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
        var shopItem = await _context.ShopItems.FirstOrDefaultAsync(x => x.Id == id);
        if (shopItem == null) throw new KeyNotFoundException($"ShopItem with ID {id} does not exist.");
        return shopItem;
    }

    public async Task<List<ShopItemModel>> GetAllShopItems()
    {
        return await _context.ShopItems.AsQueryable().ToListAsync();
    }

    public async Task CreateShopItem(ShopItemModel item)
    {
        if (item == null) throw new ArgumentNullException(nameof(item));
        await _context.ShopItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateShopItem(Guid id, ShopItemModel item)
    {
        var shopItem = await GetShopItem(id);
        if (shopItem == null) throw new KeyNotFoundException($"ShopItem with ID {id} does not exist.");

        shopItem.Price = item.Price;
        shopItem.Name = item.Name;
        shopItem.Description = item.Description;

        _context.ShopItems.Update(shopItem);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveShopItem(Guid id)
    {
        var shopItem = await GetShopItem(id);
        if (shopItem == null) throw new KeyNotFoundException($"ShopItem with ID {id} does not exist.");
        _context.ShopItems.Remove(shopItem);
        await _context.SaveChangesAsync();
    }
}
