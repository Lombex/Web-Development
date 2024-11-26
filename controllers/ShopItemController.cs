using Microsoft.AspNetCore.Mvc;

[Route("api/shopitem")]
[ApiController]
public class ShopItemController : ControllerBase
{
    private readonly IShopItemService _shopItemService;
    public ShopItemController(IShopItemService shopItemService)
    {
        _shopItemService = shopItemService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetShopItem(Guid id)
    {
        ShopItems? shopItem = await _shopItemService.GetShopItem(id);
        return Ok(shopItem);
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateShopItem(ShopItems item)
    {
        await _shopItemService.CreateShopItem(item);
        return Ok(new { message = "ShopItem created successfully!"});
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateShopItem(Guid id, [FromBody] float Price, [FromBody] string Name, [FromBody] string Description)
    {
        await _shopItemService.UpdateShopItem(id, Price, Name, Description);
        return Ok(new { message = "ShopItem updated successfully!" });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteShopItem(Guid id)
    {
        await _shopItemService.RemoveShopItem(id);
        return Ok(new { message = "ShopItem deleted successfully!" });
    }
}