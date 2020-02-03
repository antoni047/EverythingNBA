﻿namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Models;

    public interface IAllStarTeamService
    {
        Task<int> AddAllStarTeamAsync(int year, string type);

        Task<bool> DeleteAllStarTeamAsync(int allStarTeamId);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsByNameAsync (string type);

        Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsBySeasonAsync(int seasonId);

        Task<AllStarTeam> GetAllStarTeamAsync(string type, int Year);
    }
}
