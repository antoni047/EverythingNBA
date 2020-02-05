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

        Task AddPlayerAsync(int playerId, int teamId);

        Task<bool> RemovePlayerAsync(int playedId, int teamId);

        //Task AddGameAsync(int gameId, int teamId);
    }
}
