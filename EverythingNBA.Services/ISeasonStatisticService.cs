namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Services.Models.SeasonStatistic;

    public interface ISeasonStatisticService
    {
        Task<int> AddAsync(int seasonId, int teamId, int wins, int losses);

        Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int seasonId, int teamId);

        Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int id);

        Task<string> GetWinPercentageAsync(int seasonStatisticId);
    }
}
