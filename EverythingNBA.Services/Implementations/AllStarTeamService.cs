

namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;

    using EverythingNBA.Models;
    using EverythingNBA.Data;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models.AllStarTeam;

    public class AllStarTeamService : IAllStarTeamService
    {
        private readonly EverythingNBADbContext db;

        public AllStarTeamService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAllStarTeamAsync(int year, string type)
        {
            var allStarTeamObj = new AllStarTeam
            {
                Year = year,
                Type = (AllStarTeamType)Enum.Parse(typeof(AllStarTeamType), type),
            };

            this.db.AllStarTeams.Add(allStarTeamObj);
            await this.db.SaveChangesAsync();

            return allStarTeamObj.Id;
        }

        public async Task<bool> DeleteAllStarTeamAsync(int allStarTeamId)
        {
            var teamToDelete = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            if (teamToDelete == null)
            {
                return false;
            }

            this.db.AllStarTeams.Remove(teamToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsByNameAsync(string type)
        {
            var allStarTeams = await this.db.AllStarTeams.Where(ast => ast.Type.ToString() == type).ToListAsync();

            var allStarTeamServiceModels = new List<GetAllStarTeamServiceModel>();

            foreach (var team in allStarTeams)
            {
                var model = Mapper.Map<GetAllStarTeamServiceModel>(team);

                allStarTeamServiceModels.Add(model);
            }

            return allStarTeamServiceModels;
        }

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsBySeasonAsync(int Year)
        {
            var allStarTeams = await this.db.AllStarTeams.Where(ast => ast.Year == Year).ToListAsync();

            var allStarTeamServiceModels = new List<GetAllStarTeamServiceModel>();

            foreach (var team in allStarTeams)
            {
                var model = Mapper.Map<GetAllStarTeamServiceModel>(team);

                allStarTeamServiceModels.Add(model);
            }

            return allStarTeamServiceModels;
        }

        public async Task<AllStarTeam> GetAllStarTeamAsync(string type, int Year)
        {
            var allStarTeam = await this.db.AllStarTeams.Where(ast => ast.Type.ToString() == type && ast.Year == Year).FirstOrDefaultAsync();

            return allStarTeam;
        }
    }
}
