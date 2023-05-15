using BetValue.Database;
using BetValue.Models;
using Microsoft.EntityFrameworkCore;

namespace BetValue.Repos
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
        public async Task<List<CountryModel>> GetCountriesAsync()
        {
            return await context.Countries.ToListAsync();
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

        public async Task<CountryModel?> GetCountryAsync(string countryName)
        {
            return await context.Countries.FirstOrDefaultAsync(c => c.Name == countryName);
        }

        public async Task<CountryModel?> AddCountryAsync(CountryModel newCountry)
        {
            await context.Countries.AddAsync(newCountry);
            return await context.Countries.FirstOrDefaultAsync(c => c.Name == newCountry.Name);
        }
    }
}