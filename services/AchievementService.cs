using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public interface IAchievementService
{
    Task<List<Achievement>> GetUserAchievements(Guid userId);
    Task<List<Badge>> GetUserBadges(Guid userId);
    Task CheckAndAwardAchievements(User user);
    Task CheckAndAwardBadges(User user);
}

public class AchievementService : IAchievementService
{
    private readonly AppDbContext _context;

    public AchievementService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Achievement>> GetUserAchievements(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Achievements)
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user?.Achievements.ToList() ?? new List<Achievement>();
    }

    public async Task<List<Badge>> GetUserBadges(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.Badges)
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user?.Badges.ToList() ?? new List<Badge>();
    }


    public async Task CheckAndAwardAchievements(User user)
    {
        var achievements = await _context.Achievements.ToListAsync();
        foreach (var achievement in achievements)
        {
            if (!user.Achievements.Any(a => a.Id == achievement.Id) && 
                user.Points.AllTimePoints >= achievement.PointsRequired)
            {
                user.Achievements.Add(achievement);
                await _context.PointsHistory.AddAsync(new PointsHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Amount = 0,
                    Description = $"Achievement Unlocked: {achievement.Name}",
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        await _context.SaveChangesAsync();
    }

    public async Task CheckAndAwardBadges(User user)
    {
        var badges = await _context.Badges.ToListAsync();
        foreach (var badge in badges)
        {
            if (!user.Badges.Any(b => b.Id == badge.Id) && 
                user.Points.AllTimePoints >= badge.RequiredPoints)
            {
                user.Badges.Add(badge);
                await _context.PointsHistory.AddAsync(new PointsHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Amount = 0,
                    Description = $"Badge Earned: {badge.Name}",
                    Timestamp = DateTime.UtcNow
                });
            }
        }
        await _context.SaveChangesAsync();
    }
}