namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Season;

    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System;

    public interface ISeasonService
    {
        Task<int> AddAsync(int year, string titleWinner, int gamesPlayed);

        Task<bool> DeleteAsync (int seasonId);

        Task<GetSeasonDetailsServiceModel> GetDetailsByYearAsync(int year);

        Task<GetSeasonDetailsServiceModel> GetDetailsAsync(int seasonId);

        Task AddPlayoffAsync (int seasonId, int playoffId);

        Task<bool> RemovePlayoffAsync(int seasonId, int playoffId);

        Task<ICollection<GetSeasonListingServiceModel>> GetAllSeasonsAsync();

        Task EditSeasonAsync(GetSeasonDetailsServiceModel model, int seasonId);

        Task<int> GetYearAsync(int seasonId);

        Task<DateTime> GetSeasonStartDateAsync(int seasonId);

        Task<DateTime> GetSeasonEndDateAsync(int seasonId);
    }
}
