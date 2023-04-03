

using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class GameModelRepository
    {
        private readonly BetValueDbContext context;

        public GameModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<GameModel> GetGames()
        {
            return context.Games.ToList();
        }
        public async Task<List<GameModel>> GetGamesAsync()
        {
            return await context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).ToListAsync();
        }
        public GameModel? GetGame(int id)
        {
            return context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).FirstOrDefault(g => g.Id == id);
        }
        public void AddGame(GameModel game)
        {
            context.Games.Add(game);
        }
        public async Task AddGameAsync(GameModel game)
        {
            await context.Games.AddAsync(game);
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
            List<GameModel>? games = context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).Where(g => g.HomeTeam.Name.Contains(homeTeam) && g.AwayTeam.Name.Contains(awayTeam)).ToList();
            if (games != null)
            {
                foreach (GameModel game in games)
                {
                    SerieModel? serie = context.Series.FirstOrDefault(s => s.Id == game.SerieId);
                    if (serie.Year == year) return game;
                }
                return games.LastOrDefault();
            }

            return null;
        }
        public async Task<GameModel?> GetGameAsync(string homeTeam, string awayTeam)
        {
            GameModel? game = await context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).OrderBy(g => g.Id).LastOrDefaultAsync
            (g => homeTeam.Contains(g.HomeTeam.Name) && awayTeam.Contains(g.AwayTeam.Name));

            //GameModel? game = await context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).FirstOrDefaultAsync
            //(g => g.HomeTeam.ShortNames.Any(s => s == homeTeam) && g.AwayTeam.ShortNames.Any(s => s == awayTeam));

            //GameModel? game = await context.Games
            //                .Include(g => g.HomeTeam).Include(g => g.AwayTeam)
            //                .Where(g => g.HomeTeam.ShortNames.Where(s => s == homeTeam).FirstOrDefault() != null
            //                        && g.AwayTeam.ShortNames.Where(s => s == awayTeam).FirstOrDefault() != null)
            //                .FirstOrDefaultAsync();

            return game;
        }
        public async Task<GameModel?> GetGameAsync(string homeTeam, string awayTeam, int serieId)
        {
            GameModel? game = await context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).
                FirstOrDefaultAsync(g => homeTeam.Contains(g.HomeTeam.Name) && awayTeam.Contains(g.AwayTeam.Name) && g.SerieId == serieId);
            //GameModel? game = await context.Games.Include(g => g.HomeTeam).Include(g => g.AwayTeam).
            //   FirstOrDefaultAsync(g => g.HomeTeam.Name == homeTeam && g.AwayTeam.Name == awayTeam && g.SerieId == serieId);

            return game;
        }

        public async Task<GameModel?> GetGameAsync(int id)
        {
            return await context.Games.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}