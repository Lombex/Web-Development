using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;

public class AppDbContext : DbContext
{
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ShopItems> ShopItems { get; set; }
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<Badge> Badges { get; set; }
    public DbSet<Streak> Streaks { get; set; }
    public DbSet<Level> Levels { get; set; }
    public DbSet<PointsHistory> PointsHistory { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure User Points
        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Points, points =>
            {
                points.OwnsMany(p => p.Items, items =>
                {
                    items.WithOwner();
                    items.Property<Guid>("Id");
                    items.HasKey("Id");
                });
            });

        // Configure relationships
        modelBuilder.Entity<User>()
            .HasOne(u => u.Streak)
            .WithOne(s => s.User)
            .HasForeignKey<Streak>(s => s.UserId);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Achievements)
            .WithMany(a => a.Users)
            .UsingEntity(j => j.ToTable("UserAchievements"));

        modelBuilder.Entity<User>()
            .HasMany(u => u.Badges)
            .WithMany(b => b.Users)
            .UsingEntity(j => j.ToTable("UserBadges"));

        modelBuilder.Entity<User>()
            .HasMany(u => u.PointsHistory)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId);

        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.User)
            .WithMany(u => u.EventAttendances)
            .HasForeignKey(ea => ea.UserId);

        modelBuilder.Entity<EventAttendance>()
            .HasOne(ea => ea.Event)
            .WithMany(e => e.EventAttendances)
            .HasForeignKey(ea => ea.EventId);

        base.OnModelCreating(modelBuilder);
    }
}

