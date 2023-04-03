
using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
{
    public class SerieModelRepository
    {
        private readonly BetValueDbContext context;

        public SerieModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<SerieModel> GetSeries()
        {
            return context.Series.ToList();
        }
        public SerieModel? GetSerie(int id)
        {
            return context.Series.FirstOrDefault(g => g.Id == id);
        }
        public void AddSerie(SerieModel serie)
        {
            context.Series.Add(serie);
        }
        public async Task AddSerieAsync(SerieModel serie)
        {
            await context.Series.AddAsync(serie);
        }
        public void UpdateSerie(SerieModel serie)
        {
            context.Series.Update(serie);
        }
        public void RemoveSerie(SerieModel serie)
        {
            context.Series.Remove(serie);
        }

        public SerieModel? GetSerie(string name, string year)
        {
            return context.Series.Include(s => s.League).
                FirstOrDefault(s => s.League.Name == name && s.Year == year);
        }

        public async Task<SerieModel?> GetSerieAsync(LeagueModel league, string year)
        {
            return await context.Series.Include(s => s.League).FirstOrDefaultAsync(s => s.League.Id == league.Id && s.Year == year);
        }

        public Task<SerieModel?> GetSerieAsync(int serieId)
        {
            return context.Series.Include(s => s.League).FirstOrDefaultAsync(s => s.Id == serieId);
        }

        public async Task<SerieModel?> GetLastSerieAsync(int leagueId)
        {
            return await context.Series.Include(s => s.League).Where(s => s.League.Id == leagueId).OrderBy(s => s.Year).LastOrDefaultAsync();
        }
    }
}
