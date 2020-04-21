namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;

    using EverythingNBA.Services.Models.Team;

    public interface ITeamService
    {
        Task<int> AddTeamAsync(string name, IFormFile FullImageFile, IFormFile smallImageFile, string conference, 
            string venue, string instagram, string twitter);

        Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(int teamId, int year);

        Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name, int year);

        Task<TeamStandingsListingServiceModel> GetStandingsAsync(int seasonId);

        Task<bool> RemoveGameAsync(int gameId, int teamId);

        Task EditTeamAsync(GetTeamDetailsServiceModel model, int id, IFormFile fullImage, IFormFile smallImage);

        Task<ICollection<TeamListingSerivceModel>> GetAllTeamsAsync();

        Task<TeamOverviewServiceModel> GetTeamAsync(int teamId);

        Task<TeamOverviewServiceModel> GetTeamAsync(string name);
    }
}
