public class UserPointsModel
{
    public int AllTimePoints { get; set; }
    public int PointAmount { get; set; }
    public List<ShopItems> items { get; set; }
}

public interface IPointSystemService
{
    Task<int> GetPointsFromUser(User user);
    void AddUserPoints(User user, int amount);
    bool UpdateUserPoints(User user, int amount);
    float GetUserLevel(User user);
    Task<bool> BuyItem(User user, ShopItems item);
    Task<List<ShopItems>> GetAllShopItems();
}

public class PointSystemService 
{
    private readonly Dictionary<Guid, int> _userPoints = new();

    public async Task<int> GetPointsFromUser(User user)
    {
        return await Task.FromResult(_userPoints.TryGetValue(user.id, out var points) ? points : 0);
    }

    public async void AddUserPoints(User user, int amount)
    {
        if (_userPoints.ContainsKey(user.id)) _userPoints[user.id] += amount;
        else _userPoints[user.id] = amount;
        await Task.CompletedTask;
    }

    public async Task<bool> BuyItem(User user, ShopItems item)
    {
        if (_userPoints.TryGetValue(user.id, out var points) && points >= item.price)
        {
            _userPoints[user.id] -= item.price;
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    public async Task<bool> UpdateUserPoints(User user, int amount)
    {
        if (_userPoints.ContainsKey(user.id))
        {
            _userPoints[user.id] = amount;
            return await Task.FromResult(true);
        }
        return await Task.FromResult(false);
    }

    public async Task<float> GetUserLevel(User user)
    {
        if (_userPoints.TryGetValue(user.id, out var points))
        {
            float level = points / 100;
            return await Task.FromResult(level);
        }
        return await Task.FromResult(0.0f);
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