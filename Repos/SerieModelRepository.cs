using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Services
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
        public SerieModel GetLastSerie(LeagueModel league)
        {
            List<SerieModel> list = context.Series.Include(s => s.SerieMembers).ThenInclude(sm => sm.Team).Where(s => s.League.Id == league.Id).ToList();
            return list[list.Count - 1];
        }
        public SerieModel? GetSerie(int id)
        {
            return context.Series.FirstOrDefault(g => g.Id == id);
        }
        public void AddSerie(SerieModel serie)
        {
            context.Series.Add(serie);
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
    }
}
