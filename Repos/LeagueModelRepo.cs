using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class LeagueModelRepo : ILeagueModelRepo
    {
        private readonly BetValueDbContext _context;
        public LeagueModelRepo(BetValueDbContext context)
        {
            _context = context;
        }
        public void AddLeague(LeagueModel league)
        {
            _context.Leagues.Add(league);
            _context.SaveChanges();
        }
        public void DeleteLeague(int Id)
        {
            var league = _context.Leagues.Find(Id);
            if (league != null)
            {
                _context.Leagues.Remove(league);
                _context.SaveChanges();
            }
        }
        public LeagueModel? GetLeague(int Id)
        {
            return _context.Leagues.Find(Id);
        }
        public List<LeagueModel> GetLeagues()
        {
            return _context.Leagues.Include(l => l.Country).ToList();
        }
        public void UpdateLeague(LeagueModel league)
        {
            _context.Leagues.Update(league);
            _context.SaveChanges();
        }
    }
}
