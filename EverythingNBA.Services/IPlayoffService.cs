﻿namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    using EverythingNBA.Services.Models.Playoff;

    public interface IPlayoffService
    {
        Task<int> AddPlayoffAsync(int? seasonId);

        Task<bool> DeletePlayoffAsync(int playoffId);

        Task<GetPlayoffServiceModel> GetDetailsAsync(int playoffId);

        Task<GetPlayoffServiceModel> GetDetailsBySeasonAsync(int seasonId);

        Task AddSeriesAsync(int playoffId, int seriesId);
    }
}
