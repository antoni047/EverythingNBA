namespace EverythingNBA.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<int> AddTeamAsync(string name, IFormFile imageFile, string conference, string venue, string instagram, string twitter);

        Task<bool> DeleteTeamAsync(int teamId);
    }
}
