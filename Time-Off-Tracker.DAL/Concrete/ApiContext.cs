using Microsoft.EntityFrameworkCore;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.DAL.Concrete
{
    public class ApiContext:DbContext
    {
        public ApiContext(DbContextOptions options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Permission>()
                .Property(p => p.StartDate)
                .HasColumnType("timestamp");

            modelBuilder.Entity<Permission>()
                .Property(p => p.EndDate)
                .HasColumnType("timestamp");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
