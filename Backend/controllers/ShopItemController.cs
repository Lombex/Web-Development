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

    [HttpGet("Test")]
    public IActionResult TestAPI() => Ok("API is healthy!");

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShopItem(Guid id)
    {
        var shopItem = await _shopItemService.GetShopItem(id);
        return Ok(shopItem);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllShopItems()
    {
        var shopItems = await _shopItemService.GetAllShopItems();
        return Ok(shopItems);
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateShopItem([FromBody] ShopItemModel item)
    {
        if (item == null) return BadRequest(new { message = "Invalid ShopItem data." });
        await _shopItemService.CreateShopItem(item);
        return CreatedAtAction(nameof(GetShopItem), new { id = item.Id }, new { message = "ShopItem created successfully!", shopItem = item });
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateShopItem(Guid id, [FromBody] ShopItemModel item)
    {
        if (item == null) return BadRequest(new { message = "Invalid ShopItem data." });
        await _shopItemService.UpdateShopItem(id, item);
        return Ok(new { message = "ShopItem updated successfully!" });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteShopItem(Guid id)
    {
        await _shopItemService.RemoveShopItem(id);
        return Ok(new { message = "ShopItem deleted successfully!" });
    }
}