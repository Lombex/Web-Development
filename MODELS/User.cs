namespace WebApi.Models;
public class User
{
    public Guid Id { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int RecuringDays { get; set; }
    public string Role { get; set; }
}
