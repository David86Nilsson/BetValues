using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Services
{
    public class GameModelRepository
    {
        private readonly BetValueDbContext context;

        public GameModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public async Task<List<GameModel>> GetGamesAsync()
        {
            return await context.Games.ToListAsync();
        }
        public async Task<GameModel?> GetGameAsync(int id)
        {
            return await context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }
        public void AddGame(GameModel game)
        {
            context.Games.Add(game);
        }
        public void UpdateGame(GameModel game)
        {
            context.Games.Update(game);
        }
        public void RemoveGame(GameModel game)
        {
            context.Games.Remove(game);
        }

        public GameModel? GetGame(int id1, int id2, int serieId)
        {
            return context.Games.FirstOrDefault(g => g.HomeTeam.Id == id1 && g.AwayTeam.Id == id2 && g.SerieId == serieId);
        }
        public GameModel? GetGame(string homeTeam, string awayTeam, string year)
        {
            return context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).Include(g => g.Serie).
                FirstOrDefault(g => g.HomeTeam.Name == homeTeam && g.AwayTeam.Name == awayTeam && g.Serie.Year == year);
        }

        public async Task<List<GameModel>>? GetValueGamesAsync(double value, int? leagueId = null)
        {
            List<GameModel> valueList;
            if (leagueId != null)
            {
                valueList = await context.Games.
                    Include(g => g.HomeTeam).Include(g => g.AwayTeam).Include(g => g.Serie).
                    Where(g => g.BetValue > value && g.Date > DateTime.Now && g.Serie.LeagueId == leagueId).OrderBy(g => g.Date).ToListAsync();
            }
            else
            {
                valueList = await context.Games.
                    Include(g => g.HomeTeam).Include(g => g.AwayTeam).
                    Where(g => g.BetValue > value && g.Date > DateTime.Now).OrderBy(g => g.Date).ToListAsync();
            }
            return valueList;
        }

        public async Task<List<GameModel>>? GetUnplayedGamesAsync()
        {
            List<GameModel> unplayedList = await context.Games.
                Include(g => g.HomeTeam).Include(g => g.AwayTeam).
                Where(g => g.Date < DateTime.Now).OrderBy(g => g.Date).ToListAsync();
            return unplayedList;
        }
        public async Task<List<GameModel>>? GetUnplayedGamesWithOddsAsync()
        {
            List<GameModel> unplayedList = await context.Games.
                Include(g => g.HomeTeam).Include(g => g.AwayTeam).
                Where(g => g.Date > DateTime.Now && !string.IsNullOrEmpty(g.Odds1)).OrderBy(g => g.Date).ToListAsync();
            return unplayedList;
        }
    }
}
