using Microsoft.AspNetCore.Mvc;

[Route("api/points")]
 [ApiController]
    public class PointSystemController : ControllerBase
    {
        private readonly IPointSystemService _pointSystemService;

        public PointSystemController(IPointSystemService pointSystemService)
        {
            _pointSystemService = pointSystemService;
        }

        // 1. Get User Points
        [HttpGet("{userId}/points")]
        public async Task<IActionResult> GetUserPoints(Guid userId)
        {
            var points = await _pointSystemService.GetPointsFromUser(userId);
            return Ok(points);
        }

        // 2. Add Points to User
        [HttpPost("{userId}/points/add")]
        public async Task<IActionResult> AddPoints(Guid userId, [FromBody] int amount)
        {
            _pointSystemService.AddUserPoints(userId, amount);
            return Ok();
        }

        // 3. Update User Points
        [HttpPut("{userId}/points/update")]
        public async Task<IActionResult> UpdateUserPoints(Guid userId, [FromBody] int amount)
        {
            var result = await _pointSystemService.UpdateUserPoints(userId, amount);
            return result ? Ok() : NotFound();
        }

        // 4. Buy Item
        [HttpPost("{userId}/buy-item")]
        public async Task<IActionResult> BuyItem(Guid userId, [FromBody] Guid itemId)
        {
            var item = await _pointSystemService.GetItemById(itemId);
            if (await _pointSystemService.BuyItem(userId, item))
                return Ok();
            return BadRequest("Insufficient points or item not available.");
        }

        // 5. Get All Shop Items
        [HttpGet("shop/items")]
        public async Task<IActionResult> GetAllShopItems()
        {
            var items = await _pointSystemService.GetAllShopItems();
            return Ok(items);
        }

        // 6. Get User Level
        [HttpGet("{userId}/level")]
        public async Task<IActionResult> GetUserLevel(Guid userId)
        {
            var level = await _pointSystemService.GetUserLevel(userId);
            return Ok(level);
        }

        // 7. Get User's Items
        [HttpGet("{userId}/items")]
        public async Task<IActionResult> GetUserItems(Guid userId)
        {
            var items = await _pointSystemService.GetUserItems(userId);
            return Ok(items);
        }
    }
