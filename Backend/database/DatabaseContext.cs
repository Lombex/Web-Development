using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

public class AppDbContext : DbContext
{
    public DbSet<EventAttendance_model> EventAttendances { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ShopItems> ShopItems { get; set; }
    public DbSet<UserPoints_model> UserPointsModels { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=./database/database.db");
    }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure UserPointsModel as an owned type of User
        modelBuilder.Entity<User>().OwnsOne(u => u.Points, points =>
        {
            // Configure the Items property as a collection of owned entities
            points.OwnsMany(p => p.Items, item =>
            {
                // Additional configuration for ShopItems if needed
                item.WithOwner(); // This assumes ShopItems is owned by UserPointsModel
            });
        });
        modelBuilder.Entity<EventAttendance_model>().HasOne(ea => ea.User)
            .WithMany(u => u.EventAttendances).HasForeignKey(ea => ea.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventAttendance_model>().HasOne(ea => ea.Event)
            .WithMany(e => e.EventAttendances).HasForeignKey(ea => ea.EventId).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}

