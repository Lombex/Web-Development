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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShopItem(Guid id)
    {
        ShopItems? shopItem = await _shopItemService.GetShopItem(id);
        return Ok(shopItem);
    }

    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ShopItems>>> GetAllShopItems() => Ok(await _shopItemService.GetAllShopItems());

    [HttpPost("add")]
    public async Task<IActionResult> CreateShopItem([FromBody] ShopItems item)
    {
        await _shopItemService.CreateShopItem(item);
        return Ok(new { message = "ShopItem created successfully!"});
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateShopItem(Guid id, [FromBody] ShopItems items)
    {
        await _shopItemService.UpdateShopItem(id, items);
        return Ok(new { message = "ShopItem updated successfully!" });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteShopItem(Guid id)
    {
        await _shopItemService.RemoveShopItem(id);
        return Ok(new { message = "ShopItem deleted successfully!" });
    }
}