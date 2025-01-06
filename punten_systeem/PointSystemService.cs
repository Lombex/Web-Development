
public interface IPointSystemService
{
    Task<int> GetPointsFromUser(User user);
    Task AddUserPoints(User user, int amount);
    Task<bool> UpdateUserPoints(User user, int amount);
    Task<float> GetUserLevel(User user);
    Task<bool> BuyItem(User user, ShopItems item);
    // Task<List<ShopItems>> GetAllShopItems();
    // Task<ShopItems> GetItemById(Guid id);
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
        // Ensure user is not null before accessing Points
        return await Task.FromResult(user?.Points.PointAmount ?? 0);
    }

    public async Task AddUserPoints(User user, int amount)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));

        user.Points.AllTimePoints += amount;
        user.Points.PointAmount += amount;

        _context.Users.Update(user); // Update user in the database
        await _context.SaveChangesAsync(); // Save changes
    }

    public async Task<bool> BuyItem(User user, ShopItems item)
    {
        if (user == null || item == null) return false;

        if (user.Points.PointAmount >= item.Price)
        {
            user.Points.PointAmount -= item.Price;
            user.Points.Items.Add(item);

            _context.Users.Update(user); // Update user in the database
            await _context.SaveChangesAsync(); // Save changes
            return true;
        }
        return false;
    }

    public async Task<bool> UpdateUserPoints(User user, int amount)
    {
        if (user == null) return false;

        user.Points.PointAmount = amount;

        _context.Users.Update(user); // Update user in the database
        await _context.SaveChangesAsync(); // Save changes
        return true;
    }

    public async Task<float> GetUserLevel(User user)
    {
        if (user == null) return 0.0f;

        float level = user.Points.AllTimePoints / 100;
        return await Task.FromResult(level);
    }
}


// lvl (lastlvl + currentlvl) * 1.25


/*
    - Users are getting points for participating with events.
    - When user has a current streak you get bonus points

    - Users geting points for giving feedback on participated event.

    - When logging in on time you earn points.

    - After Finishing a event you earn bonus points.

    - You can trade points for functions such as.
        Dark theme, Levels.

    - You can earn badges or special roles with x amount of points.
    - Special roles and badges are shown on users profiles.

    × There must be a dashboard where the users progression is shown.
    - There must be a leaderboard for users to make it competetive.
   
    */

// TODO
// Make a function that can read and write to the database.
// Make a fuction that (Get, Add, Delete, Update) Users points.

// Make a function to trade with points for cosmetics.
// Make a list of purchasable badges / special roles.
// Make a list of purchasable cosmetics.

// Make a leaderboard for users with most amount of points.

/* -> Inherit from EventServices to check if user is participating in events.
        - Check the current streak and add x amount bonus points to users.
        - Add bonus point when feedback has been given.
        - If Event is finished you earn bonus points.
*/

/* -> Inherit from Login {Get user time}
      Time of login/logout should be handled in the Event Attandance class.
      |
      |_ Get user login/logout time from Event Attandance class and add points accordingly. 
*/