namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Models;

    public interface IAllStarTeamService
    {
        Task<int> AddAllStarTeamAsync(int year, string type, ICollection<string> playerNames);

        Task<bool> DeleteAllStarTeamAsync(int allStarTeamId);

        Task<GetAllStarTeamServiceModel> GetAllStarTeamAsync(int id);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync (string type);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(int Year);

        Task<GetAllStarTeamServiceModel> GetAllStarTeamByTypeAndYearAsync(string type, int Year);

        Task<bool> RemovePlayerAsync(int allStarTeamId, int playerId);
    }
}
