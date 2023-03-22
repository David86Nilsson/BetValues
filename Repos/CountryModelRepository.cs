using BetValue.Database;
using BetValue.Models;

namespace BetValue.Services
{
    public class CountryModelRepository
    {
        private readonly BetValueDbContext context;

        public CountryModelRepository(BetValueDbContext context)
        {
            this.context = context;
        }
        public List<CountryModel> GetCountries()
        {
            return context.Countries.ToList();
        }
        public CountryModel? GetCountry(int id)
        {
            return context.Countries.FirstOrDefault(c => c.Id == id);
        }
        public CountryModel? GetCountry(string name)
        {
            return context.Countries.FirstOrDefault(c => c.Name == name);
        }
        public void AddCountry(CountryModel country)
        {
            context.Countries.Add(country);
        }
        public void UpdateCountry(CountryModel country)
        {
            context.Countries.Update(country);
        }
        public void RemoveCountry(CountryModel country)
        {
            context.Countries.Remove(country);
        }
    }
}
