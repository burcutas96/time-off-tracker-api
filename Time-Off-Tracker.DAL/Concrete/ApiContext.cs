using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Off_Tracker.Entity.Concrete;

namespace Time_Off_Tracker.DAL.Concrete
{
    public class ApiContext:DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=time-off-tracker-database.cl7uvv8utzfs.eu-north-1.rds.amazonaws.com; Database=time-off-tracker; User Id=sa; Password=database123; TrustServerCertificate=true");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
    }
}
