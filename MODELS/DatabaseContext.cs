using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    //Eigenschappen van de database
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }
}
