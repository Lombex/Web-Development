using Microsoft.EntityFrameworkCore;
using System;
public interface IUserProgressService
{
    Task TrackProgress(User user, int points, string description);
    Task CheckAchievementsAndBadges(User user);
}

public class UserProgressService : IUserProgressService
{
    private readonly AppDbContext _context;

    public UserProgressService(AppDbContext context)
    {
        _context = context;
    }

    public async Task TrackProgress(User user, int points, string description)
    {
        await _context.PointsHistory.AddAsync(new PointsHistory
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Amount = points,
            Description = description,
            Timestamp = DateTime.UtcNow
        });

        await CheckAchievementsAndBadges(user);
        await _context.SaveChangesAsync();
    }

    public async Task CheckAchievementsAndBadges(User user)
    {
        var achievements = await _context.Achievements.ToListAsync();
        var badges = await _context.Badges.ToListAsync();

        foreach (var achievement in achievements)
        {
            if (!user.Achievements.Any(a => a.Id == achievement.Id) && 
                user.Points.AllTimePoints >= achievement.PointsRequired)
            {
                user.Achievements.Add(achievement);
            }
        }

        foreach (var badge in badges)
        {
            if (!user.Badges.Any(b => b.Id == badge.Id) && 
                user.Points.AllTimePoints >= badge.RequiredPoints)
            {
                user.Badges.Add(badge);
            }
        }
    }
}