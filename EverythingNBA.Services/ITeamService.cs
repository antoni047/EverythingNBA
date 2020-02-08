namespace EverythingNBA.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.Team;

    public interface ITeamService
    {
        Task<int> AddTeamAsync(string name, IFormFile imageFile, string conference, string venue, string instagram, string twitter);

        Task<bool> DeleteTeamAsync(int teamId);

        Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(int teamId);

        Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name);

        Task<ICollection<TeamStandingsListingServiceModel>> GetStandingsAsync(int seasonId);

        Task AddPlayerAsync(int playerId, int teamId);

        Task<bool> RemovePlayerAsync(int playedId, int teamId);

        Task<bool> AddGameAsync(int gameId, int teamId);

        Task<bool> RemoveGameAsync(int gameId, int teamId);

        Task<bool> AddTitleAsync(int teamId, int seasonId);

        Task<bool> RemoveTitleAsync(int teamId, int seasonId);

        Task<bool> AddSeasonStatistic(int teamId, int seasonStatisticId);

        Task<bool> RemoveSeasonStatistic (int teamId, int seasonStatisticId);
    }
}
