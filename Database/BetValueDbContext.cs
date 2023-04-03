using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Database
{
    public class BetValueDbContext : DbContext
    {
        public BetValueDbContext(DbContextOptions<BetValueDbContext> options) : base(options)
        {
        }
        public DbSet<LeagueModel> Leagues { get; set; }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<SerieMemberModel> SerieMembers { get; set; }
        public DbSet<SerieModel> Series { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<BetModel> Bets { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<OddsModel> Odds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameModel>()
                .HasOne(g => g.HomeTeam)
                .WithMany()
                .HasForeignKey(g => g.HomeTeamId);

            modelBuilder.Entity<GameModel>()
                .HasOne(g => g.AwayTeam)
                .WithMany()
                .HasForeignKey(g => g.AwayTeamId);
        }
    }
}
