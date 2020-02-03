namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models;
    using System.Threading.Tasks;

    public interface ISeasonService
    {
        Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId);

        Task<bool> DeleteAsync (int seasonId);

        Task<GetSeasonDetailsServiceModel> GetSeasonByYearAsync(int year);

        Task<GetSeasonDetailsServiceModel> GetSeasonByIdAsync(int seasonId);

        Task<int> AddAwardAsync(int seasonId, int Year, string winnerName, string Name);

        Task<int> AddAllStarTeamAsync(int seasonId, int Year, string type ,string player1, string player2, string player3, string player4, string player5);
    }
}
