using AlicundeApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AlicundeApi.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Fees> Fees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Bank>()
                .Property(b => b.Id)
                .ValueGeneratedOnAdd();

           modelBuilder.Entity<Fees>()
               .Property(f => f.Id)
               .ValueGeneratedOnAdd();
        }
    }
}
