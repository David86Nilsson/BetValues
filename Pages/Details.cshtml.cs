using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc;
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

        [BindProperty]
        public string? Value { get; set; }
        [BindProperty]
        public string? MaxOdds { get; set; }

        public IEnumerable<SerieMemberModel> SerieMembers { get; set; }
        public DetailsModel(UnitOfWork unitOfWork)
        {
            this.uow = (UnitOfWork)unitOfWork;

        }
        public async Task OnGet(int id)
        {
            Id = id;

            AllCountries = await uow.CountryModelRepository.GetCountriesAsync();
            AllLeagues = await uow.LeagueModelRepository.GetLeaguesAsync();
            League = AllLeagues.Where(l => l.Id == id).FirstOrDefault();
            SerieToShow = await uow.SerieModelRepository.GetLastSerieAsync(id);
            AllGames = await uow.GameModelRepository.GetGamesAsync();
            SerieMembers = await uow.SerieMemberModelRepository.GetSerieMembersAsync(SerieToShow.Id);

            ValueGames = AllGames.Where(g => g.BetValue > 0 && g.Date >= DateTime.Now.Date && g.SerieId == SerieToShow.Id).OrderBy(g => g.Date).ToList();
            UnplayedGames = AllGames.Where(g => g.Date >= DateTime.Now.Date && g.SerieId == SerieToShow.Id).OrderBy(g => g.Date).ToList();
        }
    }
}
