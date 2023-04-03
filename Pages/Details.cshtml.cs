using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class DetailsModel : PageModel
    {
        public int Id { get; set; }
        public ILeagueModelRepo _leagueModelRepo;
        public LeagueModel League;
        public UnitOfWork uow;

        public List<CountryModel> AllCountries { get; set; }
        public List<LeagueModel> AllLeagues { get; set; }
        public List<GameModel> AllGames { get; set; }
        public List<GameModel> ValueGames { get; set; }
        public List<GameModel> UnplayedGames { get; set; }
        public SerieModel SerieToShow { get; set; }
        public IEnumerable<SerieMemberModel> SerieMembers { get; set; }
        public DetailsModel(UnitOfWork unitOfWork)
        {
            this.uow = (UnitOfWork)unitOfWork;

        }
        public async Task OnGet(int id)
        {
            Id = id;
            League = uow.LeagueModelRepository.GetLeague(id);
            SerieToShow = await uow.SerieModelRepository.GetLastSerieAsync(League.Id);

            AllCountries = uow.CountryModelRepository.GetCountries();
            AllLeagues = uow.LeagueModelRepository.GetLeagues();
            AllGames = await uow.GameModelRepository.GetGamesAsync();
            SerieMembers = await uow.SerieMemberModelRepository.GetSerieMembersAsync(SerieToShow.Id);

            ValueGames = AllGames.Where(g => g.BetValue > 0 && g.Date > DateTime.Now && g.SerieId == SerieToShow.Id).ToList();
            UnplayedGames = AllGames.Where(g => g.Date > DateTime.Now && g.BetValue > -10 && g.SerieId == SerieToShow.Id).ToList();
        }
    }
}
