namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Season;

    using System.Threading.Tasks;
    using System.Collections.Generic;

    public interface ISeasonService
    {
        Task<int> AddAsync(int year, string titleWinner, int gamesPlayed);

        Task<bool> DeleteAsync (int seasonId);

        Task<GetSeasonDetailsServiceModel> GetDetailsByYearAsync(int year);

        Task<GetSeasonDetailsServiceModel> GetDetailsAsync(int seasonId);

        Task AddAwardAsync(int seasonId, int awardId);

        Task<bool> RemoveAwardAsync(int seasonId, int awardId);

        Task AddAllStarTeamAsync(int seasonId, int allStarTeamId);

        Task<bool> RemoveAllStarTeamAsync(int seasonId, int allStarTeamId);

        Task AddPlayoffAsync (int seasonId, int playoffId);

        Task<bool> RemovePlayoffAsync(int seasonId, int playoffId);

        Task AddSeasonStatisticAsync (int seasonId, int seasonStatisticId);

        Task<bool> RemoveSeasonStatisticAsync(int seasonId, int seasonStatisticId);

        Task AddGameAsync(int seasonId, int gameId);

        Task<bool> RemoveGameAsync(int seasonId, int gameId);

        Task<ICollection<GetSeasonListingServiceModel>> GetAllSeasonsAsync();

        Task EditSeasonAsync(GetSeasonDetailsServiceModel model, int seasonId);

        Task<int> GetYearAsync(int seasonId);
    }
}
