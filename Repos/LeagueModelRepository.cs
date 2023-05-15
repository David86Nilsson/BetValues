using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class LeagueModelRepository
    {
        private readonly BetValueDbContext context;

        public LeagueModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<LeagueModel> GetLeagues()
        {
            return context.Leagues.ToList();
        }
        public async Task<List<LeagueModel>> GetLeaguesAsync()
        {
            return await context.Leagues.ToListAsync();
        }
        public LeagueModel? GetLeague(int id)
        {
            return context.Leagues.FirstOrDefault(l => l.Id == id);
        }
        public LeagueModel? GetLeague(string name)
        {
            return context.Leagues.FirstOrDefault(l => l.Name.ToLower() == name.ToLower());
        }
        public async Task<LeagueModel?> GetLeagueAsync(string name)
        {
            return await context.Leagues.FirstOrDefaultAsync(l => l.Name.ToLower() == name.ToLower());
        }
        public async Task<LeagueModel?> GetLeagueAsync(int id)
        {
            return await context.Leagues.FirstOrDefaultAsync(l => l.Id == id);
        }
        public void AddLeague(LeagueModel league)
        {
            context.Leagues.Add(league);
        }
        public async Task AddLeagueAsync(LeagueModel league)
        {
            await context.Leagues.AddAsync(league);
        }
        public void UpdateLeague(LeagueModel league)
        {
            context.Leagues.Update(league);
        }
        public void RemoveLeague(LeagueModel league)
        {
            context.Leagues.Remove(league);
        }
    }
}