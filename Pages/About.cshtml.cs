using BetValue.Models;
using BetValue.Repos;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BetValue.Pages
{
    public class AboutModel : PageModel
    {
        private readonly UnitOfWork uow;

        public List<LeagueModel> AllLeagues { get; private set; }

        public AboutModel(UnitOfWork unitOfWork)
        {
            this.uow = unitOfWork;
        }

        public void OnGet()
        {
            //AllLeagues = uow.LeagueModelRepository.GetLeagues();
        }
    }
}
