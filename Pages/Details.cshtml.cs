using BetValue.Models;
using BetValue.Repos;
using BetValue.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class DetailsModel : PageModel
    {
        public int Id { get; set; }
        public ILeagueModelRepo _leagueModelRepo;
        public LeagueModel League;
        public UnitOfWork uow;
        public List<LeagueModel> AllLeagues { get; set; }
        public List<GameModel> ValueGames { get; set; }
        public List<GameModel> UnplayedGames { get; set; }
        public SerieModel SerieToShow { get; set; }
        public IEnumerable<SerieMemberModel> serieMembers { get; set; }
        public DetailsModel(IUnitOfWork unitOfWork)
        {
            this.uow = (UnitOfWork)unitOfWork;

        }
        public async Task OnGet(int id)
        {
            Id = id;
            League = uow.LeagueModelRepository.GetLeague(id);
            AllLeagues = uow.LeagueModelRepository.GetLeagues();
            ValueGames = await uow.GameModelRepository.GetValueGamesAsync(double.Parse("0,1"), id);
            UnplayedGames = await uow.GameModelRepository.GetUnplayedGamesWithOddsAsync();
            SerieToShow = uow.SerieModelRepository.GetLastSerie(League);
            serieMembers = SerieToShow.SerieMembers.OrderBy(sm => sm.Points);
        }
    }
}
