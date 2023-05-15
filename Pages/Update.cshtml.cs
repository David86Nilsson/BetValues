using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class UpdateModel : PageModel
    {
        //public UnitOfWork unitOfWork;
        //public WebScraper webScraper;
        public UpdateModel()
        {
            //    this.unitOfWork = unitOfWork;
            //    this.webScraper = new(unitOfWork);
        }
        public async Task OnGet()
        {
            //    await webScraper.UpdateAllOddsAsync();
        }
    }
}

