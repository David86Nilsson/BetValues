using BetValue.Models;

namespace BetValue.Repos
{
    public interface ILeagueModelRepo
    {
        public List<LeagueModel> GetLeagues();
        public LeagueModel GetLeague(int Id);
        public void AddLeague(LeagueModel league);
        public void UpdateLeague(LeagueModel league);
        public void DeleteLeague(int Id);
    }
}
