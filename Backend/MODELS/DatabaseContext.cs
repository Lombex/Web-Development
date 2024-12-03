using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<EventAttendance> EventAttendances { get; set; }
    public DbSet<Attendance> Attendances { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; } 
    public DbSet<UserPointsModel> UserPointsModels { get; set; }
    public DbSet<ShopItemModel> ShopItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured) optionsBuilder.UseSqlite("Data Source=Database.db"); 
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventAttendance>().HasOne(ea => ea.User).WithMany(u => u.EventAttendances)
            .HasForeignKey(ea => ea.UserId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EventAttendance>().HasOne(ea => ea.Event).WithMany(e => e.EventAttendances)
            .HasForeignKey(ea => ea.EventId).OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}