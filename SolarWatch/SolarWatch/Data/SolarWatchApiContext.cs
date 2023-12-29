using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Data;

public class SolarWatchApiContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public DbSet<City>? Cities { get; set; }
    public DbSet<SunriseSunset>? SunriseSunsets { get; init; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<City>()
            .HasIndex(u => u.Id)
            .IsUnique();
    
        builder.Entity<City>()
            .HasData(
                new City { Id = 1, Name = "London", Latitude = 51.509865, Longitude = -0.118092, State = "England", Country = "GB"},
                new City { Id = 2, Name = "Budapest", Latitude = 47.497913, Longitude = 19.040236, State = "", Country = "HU"},
                new City { Id = 3, Name = "Paris", Latitude = 48.864716, Longitude = 2.349014, State = "Ile-de-France", Country = "FR" }
            );
        
        builder.Entity<SunriseSunset>()
            .HasIndex(u => u.Id)
            .IsUnique();
        
        base.OnModelCreating(builder);
    }
}

