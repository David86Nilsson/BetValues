using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BetValue.Pages
{
    public class IndexModel : PageModel
    {
        public UnitOfWork uow;

        //public readonly IUnitOfWork uow;

        public List<LeagueModel>? AllLeagues { get; set; }
        public List<GameModel>? AllGames { get; set; }
        public List<GameModel>? ValueGames { get; set; }
        public List<GameModel>? UnplayedGames { get; set; }
        public List<CountryModel>? AllCountries { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Please enter a value")]
        public string? Value { get; set; }
        [BindProperty]
        public string? MaxOdds { get; set; }

        public IndexModel(UnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }
        public async Task OnGet()
        {
            AllLeagues = uow.LeagueModelRepository.GetLeagues();
            AllCountries = uow.CountryModelRepository.GetCountries();
            AllGames = await uow.GameModelRepository.GetGamesAsync();
            ValueGames = AllGames.Where(g => g.BetValue > 0.1).ToList();
            UnplayedGames = AllGames.Where(g => g.IsPlayed == false && g.BetValue > -10).ToList();
        }
        public async Task<IActionResult> OnPost()
        {
            if (ModelState.IsValid)
            {
                ValueGames = AllGames.Where(g => g.BetValue > (double.Parse(Value.Replace('.', ',')))).ToList();
            }
            else
            {
                ValueGames = AllGames.Where(g => g.BetValue > (double.Parse("0,1"))).ToList();
            }
            UnplayedGames = AllGames.Where(g => g.IsPlayed == false && g.BetValue > -10).ToList();
            AllLeagues = uow.LeagueModelRepository.GetLeagues();
            AllCountries = uow.CountryModelRepository.GetCountries();
            return Page();
        }
    }
}