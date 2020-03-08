
namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Player;

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, string position,
            bool isStarter, /*IFormFile pictureFile, */int shirtNumber, string instagramLink, string twitterLink);

        Task<bool> DeletePlayerAsync(int playerId);

        Task<ICollection<string>> GetAllPlayersAsync();

        Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(int id);

        Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(string name);

        Task<PlayerSeasonStatisticServiceModel> GetSeasonStatistics(int playerId, int seasonId);

        Task<bool> AddGameStatistic(int playerId, int gameStatisticId);

        Task<bool> RemoveGameStatisticStatistic(int playerId, int gameStatisticId);

        Task<bool> AddAward(int playerId, int awardId);

        Task<bool> RemoveAward(int playerId, int awardId);

        Task EditPlayerAsync(PlayerDetailsServiceModel model, int playerId);

        Task<PlayerCareerStatisticServiceModel> GetCareerStatistics(int playerId);

        Task<ICollection<PlayerRecentGamesListingServiceModel>> GetRecentGamesAsync(int playerId);

        Task<PlayerAccomplishmentsListingServiceModel> GetPlayerAccomplishentsAsync(int playerId);

        Task<ICollection<TeamPlayerOverviewServiceModel>> GetAllPlayersFromTeam(int teamId);

        Task<bool> RemoveAllStarTeam(string playerName, int allStarTeamId);

        Task<bool> AddAllStarTeam(string playerName, int allStarTeamId);
    }
}
