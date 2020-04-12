namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Player;

    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<string> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, string position,
            bool isStarter, IFormFile pictureFile, int shirtNumber, string instagramLink, string twitterLink);

        Task<bool> DeletePlayerAsync(int playerId);

        Task<ICollection<string>> GetAllPlayersAsync(int page = 1);

        Task<int> TotalPlayers();

        Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(int id);

        Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(string name);

        Task<PlayerSeasonStatisticServiceModel> GetSeasonStatistics(int playerId, int seasonId);

        Task EditPlayerAsync(PlayerDetailsServiceModel model, int playerId, IFormFile image);

        Task<PlayerCareerStatisticServiceModel> GetCareerStatistics(int playerId);

        Task<ICollection<PlayerRecentGamesListingServiceModel>> GetRecentGamesAsync(int playerId);

        Task<PlayerAccomplishmentsListingServiceModel> GetPlayerAccomplishentsAsync(int playerId);

        Task<ICollection<TeamPlayerOverviewServiceModel>> GetAllPlayersFromTeam(int teamId);

        Task<bool> AddPlayerToTeamAsync(int teamId, int playerId);

        Task<bool> RemovePlayerFromTeamAsync(int teamId, int playerId);
    }
}
