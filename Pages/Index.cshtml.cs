using BetValue.Models;
using BetValue.Repos;
using BetValue.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class IndexModel : PageModel
    {
        public UnitOfWork uow;

        //public readonly IUnitOfWork uow;

        public List<LeagueModel> AllLeagues { get; set; }
        public List<GameModel> ValueGames { get; set; }
        public List<GameModel> UnplayedGames { get; set; }

        [BindProperty]
        public string Value { get; set; } = "0,1";

        public IndexModel(IUnitOfWork unitOfWork)
        {
            this.uow = (UnitOfWork)unitOfWork;
        }
        public async Task OnGet()
        {
            AllLeagues = uow.LeagueModelRepository.GetLeagues();
            ValueGames = await uow.GameModelRepository.GetValueGamesAsync(double.Parse(Value));
            UnplayedGames = await uow.GameModelRepository.GetUnplayedGamesWithOddsAsync();
        }
        public async Task OnPost()
        {
            ValueGames = await uow.GameModelRepository.GetValueGamesAsync(double.Parse(Value));
        }
    }
}