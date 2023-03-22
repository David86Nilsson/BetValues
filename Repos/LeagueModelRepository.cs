using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Services
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
            return context.Leagues.Include(l => l.Series).Include(l => l.Country).ToList();
        }
        public LeagueModel? GetLeague(int id)
        {
            return context.Leagues.FirstOrDefault(g => g.Id == id);
        }
        public LeagueModel? GetLeague(string name)
        {
            return context.Leagues.FirstOrDefault(g => g.Name == name);
        }
        public void AddLeague(LeagueModel league)
        {
            context.Leagues.Add(league);
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
