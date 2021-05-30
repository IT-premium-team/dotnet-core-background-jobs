using BackgroundTest.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTest.Data.Context
{
    public class MainDbContext : DbContext
    {
        public DbSet<ManagerChange> ManagerChanges { get; set; }
        public MainDbContext(DbContextOptions options) : base(options) { }
    }
}
