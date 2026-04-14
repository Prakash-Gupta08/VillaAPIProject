using Microsoft.EntityFrameworkCore;
using VillaWebAPI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VillaWebAPI.Data
{
    public class MyDBContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Villa> Villa { get; set; }
        public DbSet<User> Users  { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Royal Villa",
                    Details = "Luxurious villa with stunning ocean",
                    Rate = 500.0,
                    Sqft = 2500,
                    Occupancy = 6,
                    ImageUrl = "https://dotnetexample.swagger,jsx",
                    CreatedDate = new DateTime(2026, 4,13),
                    UpdatedDate = new DateTime(2026, 4,13),
                },
                new Villa
                {
                    Id = 4,
                    Name = "Raj Villa",
                    Details = "Luxurious villa",
                    Rate = 500.0,
                    Sqft = 2500,
                    Occupancy = 6,
                    ImageUrl = "https://dotnetexample.swagger,jsx",
                    CreatedDate = new DateTime(2026, 4, 13),
                    UpdatedDate = new DateTime(2026, 4, 13),
                }
                );
        }
    }
}
