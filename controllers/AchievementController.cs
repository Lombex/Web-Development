using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
[ApiController]
[Route("api/achievements")]
public class AchievementController : ControllerBase
{
    private readonly IAchievementService _achievementService;
    private readonly IUserService _userService;

    public AchievementController(IAchievementService achievementService, IUserService userService)
    {
        _achievementService = achievementService;
        _userService = userService;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserAchievements(Guid userId)
    {
        var achievements = await _achievementService.GetUserAchievements(userId);
        return Ok(achievements);
    }

    [HttpGet("badges/user/{userId}")]
    public async Task<IActionResult> GetUserBadges(Guid userId)
    {
        var badges = await _achievementService.GetUserBadges(userId);
        return Ok(badges);
    }
}