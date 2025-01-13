using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

public class AddPointsRequest
{
    public int Amount { get; set; }
    public string Reason { get; set; }
}
public interface IPointSystemService
{
    Task<int> GetPointsFromUser(User user);
    Task AddUserPoints(User user, int amount, string reason);
    Task<bool> UpdateUserPoints(User user, int amount);
    Task<float> GetUserLevel(User user);
    Task<bool> BuyItem(User user, ShopItems item);
    Task<List<PointsHistory>> GetPointsHistory(Guid userId);
    Task AddEventPoints(User user, Event @event, bool providedFeedback);
    Task<Streak> UpdateStreak(User user);
    Task<int> CalculateLevel(int points);
}

public class PointSystemService : IPointSystemService
{
    private readonly AppDbContext _context;
    private readonly IUserProgressService _userProgressService;

    public PointSystemService(AppDbContext context, IUserProgressService userProgressService)
    {
        _context = context;
        _userProgressService = userProgressService;
    }

public async Task AddUserPoints(User user, int amount, string reason)
{

    user.Points.PointAmount += amount;
    user.Points.AllTimePoints += amount;
    user.Points.LastPointsEarned = DateTime.UtcNow;

    // Track progress based on the provided amount and reason.
    await _userProgressService.TrackProgress(user, amount, reason);

    // Save changes to the database.
    await _context.SaveChangesAsync();
}

    public async Task<int> GetPointsFromUser(User user)
    {
        return await Task.FromResult(user.Points.PointAmount);
    }


    public async Task<bool> UpdateUserPoints(User user, int amount)
    {
        user.Points.PointAmount = amount;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<float> GetUserLevel(User user)
    {
        float level = user.Points.AllTimePoints / 1000f; // Every 1000 points is a level
        return await Task.FromResult(level);
    }

    public async Task<bool> BuyItem(User user, ShopItems item)
    {
        if (user.Points.PointAmount >= item.Price)
        {
            user.Points.PointAmount -= item.Price;
            user.Points.Items.Add(item);
            
            await _context.PointsHistory.AddAsync(new PointsHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Amount = -item.Price,
                Description = $"Purchased: {item.Name}",
                Timestamp = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<List<PointsHistory>> GetPointsHistory(Guid userId)
    {
        return await _context.PointsHistory
            .Where(ph => ph.UserId == userId)
            .OrderByDescending(ph => ph.Timestamp)
            .ToListAsync();
    }

    public async Task AddEventPoints(User user, Event @event, bool providedFeedback)
    {
        int points = @event.PointsReward;
        if (providedFeedback) points += 50; // Bonus for feedback
        if (@event.IsCompleted) points += @event.BonusPoints;

        await AddUserPoints(user, points, $"Participated in event: {@event.Title}");
        await UpdateStreak(user);
    }

    public async Task<Streak> UpdateStreak(User user)
    {
        var streak = await _context.Streaks
            .FirstOrDefaultAsync(s => s.UserId == user.Id);

        if (streak == null)
        {
            streak = new Streak { 
                Id = Guid.NewGuid(), 
                UserId = user.Id,
                CurrentStreak = 1,
                HighestStreak = 1,
                LastAttendance = DateTime.UtcNow
            };
            _context.Streaks.Add(streak);
        }
        else
        {
            var lastAttendance = streak.LastAttendance;
            var today = DateTime.UtcNow.Date;

            if (lastAttendance.Date == today) return streak;

            if (lastAttendance.Date == today.AddDays(-1))
            {
                streak.CurrentStreak++;
                if (streak.CurrentStreak > streak.HighestStreak)
                {
                    streak.HighestStreak = streak.CurrentStreak;
                }
            }
            else if (lastAttendance.Date != today)
            {
                streak.CurrentStreak = 1;
            }

            streak.LastAttendance = today;
        }

        await _context.SaveChangesAsync();
        return streak;
    }


    public async Task<int> CalculateLevel(int points)
    {
        return await Task.FromResult(Math.Max(1, points / 1000));
    }
}