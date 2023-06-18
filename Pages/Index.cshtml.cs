using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class IndexModel : PageModel
    {
        public UnitOfWork uow;

        //public readonly IUnitOfWork uow;

        public List<LeagueModel>? AllLeagues { get; set; } = new();
        public List<GameModel>? AllGames { get; set; } = new();
        public List<GameModel>? ValueGames { get; set; } = new();
        public List<GameModel>? UnplayedGames { get; set; } = new();
        public List<CountryModel>? AllCountries { get; set; } = new();

        [BindProperty]
        public string? Value { get; set; }
        [BindProperty]
        public string? MaxOdds { get; set; }

        public IndexModel()
        {
            //this.uow = (UnitOfWork)unitOfWork;
        }
        public async Task OnGet()
        {
            //AllLeagues = await uow.LeagueModelRepository.GetLeaguesAsync();
            //AllCountries = await uow.CountryModelRepository.GetCountriesAsync();
            //AllGames = await uow.GameModelRepository.GetUnplayedGamesWithOddsAsync();
            //ValueGames = AllGames.Where(g => g.Date >= DateTime.Now.Date && g.BetValue > 0.2).OrderBy(g => g.Date).ToList();
            //UnplayedGames = AllGames.Where(g => g.Date >= DateTime.Now.Date && g.BetValue > -10).OrderBy(g => g.Date).ToList();
        }
        public async Task<IActionResult> OnPost()
        {
            //AllLeagues = uow.LeagueModelRepository.GetLeagues();
            //AllCountries = uow.CountryModelRepository.GetCountries();
            //AllGames = await uow.GameModelRepository.GetUnplayedGamesWithOddsAsync();
            //if (string.IsNullOrEmpty(Value))
            //{
            //    Value = "0.2";
            //}
            //if (string.IsNullOrEmpty(MaxOdds))
            //{
            //    MaxOdds = "10";
            //}

            //ValueGames = AllGames.Where(g => g.Date >= DateTime.Now.Date && g.BetValue > double.Parse(Value.Replace(',', '.')) && GetValueOdds(g) <= double.Parse(MaxOdds.Replace(',', '.'))).OrderBy(g => g.Date).ToList();
            //UnplayedGames = AllGames.Where(g => g.Date >= DateTime.Now.Date && g.IsPlayed == false && g.BetValue > -10).OrderBy(g => g.Date).ToList();

            return Page();
        }

        public double GetValueOdds(GameModel game)
        {
            double odds = 0;
            var oddsList = uow.OddsModelRepository.GetAllOddsInGame(game.Id);
            if (game.WhatBetHasValue == "1")
            {
                foreach (var oddsModel in oddsList)
                {
                    if (oddsModel.HomeWin > odds) odds = oddsModel.HomeWin;
                }

            }
            else if (game.WhatBetHasValue == "2")
            {
                foreach (var oddsModel in oddsList)
                {
                    if (oddsModel.AwayWin > odds) odds = oddsModel.AwayWin;
                }
            }
            return odds;
        }
    }
}