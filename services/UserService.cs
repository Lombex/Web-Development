public interface IUserService
{
    Task<User?> GetUserAsync(Guid id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User?> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
}
public class UserService : IUserService
{
    private readonly List<User> _users = new();

    public async Task<User?> GetUserAsync(Guid id)
    {
        return await Task.FromResult(_users.FirstOrDefault(u => u.Id == id));
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await Task.FromResult(_users);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _users.Add(user);
        return await Task.FromResult(user);
    }

    public async Task<User?> UpdateUserAsync(Guid id, User user)
    {
        var index = _users.FindIndex(u => u.Id == id);
        if (index == -1) return null;
        
        _users[index].Firstname = user.Firstname;
        _users[index].Email = user.Email;
        return await Task.FromResult(_users[index]);
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return false;
        
        _users.Remove(user);
        return await Task.FromResult(true);
    }
}