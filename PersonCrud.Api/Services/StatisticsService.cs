using Microsoft.EntityFrameworkCore;
using PersonCrud.Api.Data;
using PersonCrud.Api.Enums;
using PersonCrud.Api.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetPercentByCountryAsync(string country)
        {
            var totalPersons = await _context.Persons.CountAsync();
            var argentines = await _context.Persons.Where(p => p.Country.ToLower().Equals(country.ToLower())).CountAsync();

            var percentage = (argentines * 100) / totalPersons;
            return percentage;
        }

        public async Task<int> GetTotalByGenderAsync(Gender gender)
        {
            var count = await _context.Persons.Where(p => p.Gender == gender).CountAsync();
            return count;
        }
    }
}
