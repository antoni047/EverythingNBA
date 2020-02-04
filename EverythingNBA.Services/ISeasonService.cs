namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.Season;

    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface ISeasonService
    {
        Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId, int gamesPlayed);

        Task<bool> DeleteAsync (int seasonId);

        Task<GetSeasonDetailsServiceModel> GetSeasonByYearAsync(int year);

        Task<GetSeasonDetailsServiceModel> GetSeasonByIdAsync(int seasonId);

        Task AddAwardAsync(int seasonId, int awardId);

        Task AddAllStarTeamAsync(int seasonId, int allStarTeamId);

        Task AddPlayoffAsync (int seasonId, int playoffId);

        Task AddSeasonStatisticAsync (int seasonId, int seasonStatisticId);

        Task AddGameAsync(int seasonId, int gameId);

        Task<ICollection<GetSeasonListingServiceModel>> GetAllSeasonsAsync();
    }
}
