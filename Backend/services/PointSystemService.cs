
using Microsoft.EntityFrameworkCore;
using System.Linq;

public interface IPointSystemService
{
    Task<int> GetUserPointAmount(Guid Id);  
    Task AddPointsToUser(Guid userId, int Amount);
    Task UpdateUserPoint(Guid userId, int Amount);
}

public class PointSystemService : IPointSystemService
{
    private readonly AppDbContext _context;
    public PointSystemService(AppDbContext context)
    {
        _context = context; 
    }

    public async Task<int> GetUserPointAmount(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        if (user == null) throw new NullReferenceException("User does not exist!");
        return user.Points.PointAmount;
    }

    public async Task AddPointsToUser(Guid userId, int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be a positive integer!");
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) throw new NullReferenceException("User does not exist!");
        user.Points.PointAmount += amount;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserPoint(Guid userId, int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be a positive integer!");
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null) throw new NullReferenceException("User does not exist!");
        user.Points.PointAmount = amount;
        await _context.SaveChangesAsync();
    }
}
