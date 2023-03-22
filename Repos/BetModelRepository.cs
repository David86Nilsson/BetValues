using BetValue.Database;
using BetValue.Models;

namespace BetValue.Services
{
    public class BetModelRepository
    {
        private readonly BetValueDbContext context;

        public BetModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<BetModel> GetBets()
        {
            return context.Bets.ToList();
        }
        public BetModel? GetBet(int id)
        {
            return context.Bets.FirstOrDefault(b => b.Id == id);
        }
        public void AddBet(BetModel bet)
        {
            context.Bets.Add(bet);
        }
        public void UpdateBet(BetModel bet)
        {
            context.Bets.Update(bet);
        }
        public void RemoveBet(BetModel bet)
        {
            context.Bets.Remove(bet);
        }
    }
}
