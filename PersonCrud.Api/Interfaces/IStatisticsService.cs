using PersonCrud.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonCrud.Api.Interfaces
{
    public interface IStatisticsService
    {
        Task<int> GetTotalByGenderAsync(Gender gender);
        Task<decimal> GetPercentByCountryAsync(string country);
    }
}
