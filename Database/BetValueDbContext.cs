using BetValue.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BetValue.Database
{
    public class BetValueDbContext : DbContext
    {
        public BetValueDbContext(DbContextOptions<BetValueDbContext> options) : base(options)
        {
        }
        public DbSet<GameModel> Games { get; set; }
        public DbSet<LeagueModel> Leagues { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<SerieModel> Series { get; set; }
        public DbSet<SerieMemberModel> SerieMembers { get; set; }
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<BetModel> Bets { get; set; }

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
            modelBuilder.Entity<TeamModel>()
            .Property(x => x.ShortNames)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList())
            .Metadata
            .SetValueComparer(new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => (List<string>)c.ToList()));
        }
    }
}
