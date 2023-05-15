using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class OddsModelRepository
    {
        private readonly BetValueDbContext context;

        public OddsModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<OddsModel> GetOdds()
        {
            return context.Odds.ToList();
        }
        public OddsModel? GetOdds(int id)
        {
            return context.Odds.FirstOrDefault(o => o.Id == id);
        }
        public async Task<OddsModel?> GetOddsAsync(int gameId, string operatorName)
        {
            return await context.Odds.Include(o => o.Game).FirstOrDefaultAsync(o => o.Game.Id == gameId && o.Operator == operatorName);
        }
        public void AddOdds(OddsModel odds)
        {
            context.Odds.Add(odds);
        }
        public async Task AddOddsAsync(OddsModel odds)
        {
            await context.Odds.AddAsync(odds);
        }
        public void UpdateOdds(OddsModel odds)
        {
            context.Odds.Update(odds);
        }
        public void RemoveOdds(OddsModel odds)
        {
            context.Odds.Remove(odds);
        }

        public async Task<bool> DoesOddsExistAsync(GameModel game, string company)
        {
            OddsModel? existingOdds = await context.Odds.FirstOrDefaultAsync(o => o.GameId == game.Id && o.Operator == company);
            if (existingOdds == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<OddsModel>?> GetAllOddsInGameAsync(GameModel game)
        {
            return await context.Odds.Include(o => o.Game).Where(o => o.GameId == game.Id).ToListAsync();
        }
        public List<OddsModel>? GetAllOddsInGame(int id)
        {
            return context.Odds.Include(o => o.Game).Where(o => o.GameId == id).ToList();
        }

        public async Task<List<OddsModel>?> GetOddsFromGame(string sign, int gameId)
        {
            var odds = context.Odds.Include(o => o.Game).Where(o => o.GameId == gameId).ToList();
            return odds;
        }
    }
}