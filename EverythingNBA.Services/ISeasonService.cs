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
    }
}
