using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

public class AppDbContext : DbContext
{
    // Properties for the database sets
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<Event> Events { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ShopItems> ShopItems { get; set; }
    public DbSet<UserPointsModel> UserPointsModels { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite("Data Source=app.db");
        }
    }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure UserPointsModel as an owned type of User
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Points, points =>
            {
                // Configure the Items property as a collection of owned entities
                points.OwnsMany(p => p.Items, item =>
                {
                    // Additional configuration for ShopItems if needed
                    item.WithOwner(); // This assumes ShopItems is owned by UserPointsModel
                });
            });
        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.User)
            .WithMany(u => u.EventAttendances)
            .HasForeignKey(ea => ea.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.Event)
            .WithMany(e => e.EventAttendances)
            .HasForeignKey(ea => ea.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}

