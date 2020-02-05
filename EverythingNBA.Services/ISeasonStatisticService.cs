namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Services.Models;

    public interface ISeasonStatisticService
    {
        Task<int> AddAsync(int seasonId, int wins, int losses);

        Task<bool> DeleteAsync(int seasonStatisticId);

        Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int seasonId, int teamId);

        Task<GetSeasonStatisticDetailsServiceModel> GetDetailsAsync(int id);

        Task<string> GetWinPercentageAsync(int seasonStatisticId);

        Task AddGameAsync(int id, bool isWon);
    }
}
