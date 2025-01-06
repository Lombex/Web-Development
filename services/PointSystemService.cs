public interface IPointSystemService
{
    Task<int> GetPointsFromUser(User user);
    void AddUserPoints(User user, int amount);
    Task<bool> UpdateUserPoints(User user, int amount);
    Task<float> GetUserLevel(User user);
    Task<bool> BuyItem(User user, ShopItems item);
}

public class PointSystemService : IPointSystemService
{
    private readonly AppDbContext _context;

    public PointSystemService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetPointsFromUser(User user)
    {
        var userModel = user.Points.PointAmount;
        return await Task.FromResult(userModel);
    }

    public void AddUserPoints(User user, int amount)
    {
        user.Points.AllTimePoints += amount;
        user.Points.PointAmount += amount;
        _context.SaveChanges(); // Synchronous save
    }

    public async Task<bool> UpdateUserPoints(User user, int amount)
    {
        if (user != null)
        {
            user.Points.PointAmount = amount;
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<float> GetUserLevel(User user)
    {
        if (user != null)
        {
            float level = user.Points.AllTimePoints / 100;
            return await Task.FromResult(level);
        }
        return await Task.FromResult(0.0f);
    }

    public async Task<bool> BuyItem(User user, ShopItems item)
    {
        if (user.Points.PointAmount >= item.Price)
        {
            user.Points.PointAmount -= item.Price;
            user.Points.Items.Add(item);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }
}

// lvl (lastlvl + currentlvl) * 1.25

