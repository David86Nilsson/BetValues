using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class UpdateModel : PageModel
    {
        public UnitOfWork unitOfWork;
        public WebScraper webScraper;
        public string NotFoundGames { get; set; }
        public string UpdatedOdds { get; set; }
        public string text { get; set; }
        public UpdateModel(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this.webScraper = new(unitOfWork);
            NotFoundGames = "";
            UpdatedOdds = "";
            text = "";
        }
        public async Task OnGet()
        {
            await webScraper.UpdateAllChancesAsync();
            //await webScraper.UpdateAllOddsAsync();
            NotFoundGames = webScraper.NotFoundGames.ToString();
            //UpdatedOdds = webScraper.UpdatedOdds.ToString();
            text = webScraper.text;

        }
    }
}

