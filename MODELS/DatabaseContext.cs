using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ShopItems> ShopItems { get; set; }
    public DbSet<UserPointsModel> UserPointsModels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Example configuration for relationships and owned types
        modelBuilder.Entity<User>().OwnsOne(u => u.Points, points =>
        {
            // ShopItems is owned by UserPointsModel
            points.OwnsMany(p => p.Items, item =>
            {
                item.WithOwner(); // Ensures the relationship is correctly defined
            });
        });

        modelBuilder.Entity<EventAttendance>().HasOne(ea => ea.User).WithMany(u => u.EventAttendances)
            .HasForeignKey(ea => ea.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventAttendance>().HasOne(ea => ea.Event).WithMany(e => e.EventAttendances)
            .HasForeignKey(ea => ea.EventId).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}