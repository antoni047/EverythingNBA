namespace EverythingNBA.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using EverythingNBA.Services.Models.AllStarTeam;

    public interface IAllStarTeamService
    {
        Task<int?> AddAllStarTeamAsync(int year, string type, List<string> playerNames);

        Task<bool> DeleteAllStarTeamAsync(int allStarTeamId);

        Task<GetAllStarTeamServiceModel> GetAllStarTeamAsync(int id);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(string type);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(int Year);

        Task<GetAllStarTeamServiceModel> GetAllStarTeamByTypeAndYearAsync(string type, int Year);
    }
}
