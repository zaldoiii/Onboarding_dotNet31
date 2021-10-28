using Microsoft.EntityFrameworkCore;
using DAL.Model;
using System.Security.Cryptography.X509Certificates;

namespace DAL
{
    public class SoccerDbContext: DbContext
    {
        public SoccerDbContext(DbContextOptions<SoccerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<SoccerTeam>().HasIndex(X => X.TeamId).IsUnique();
        }

        public DbSet<SoccerTeam> SoccerTeams { get; set; }
        public DbSet<SoccerCountry> SoccerCountries { get; set; }

    }
}
