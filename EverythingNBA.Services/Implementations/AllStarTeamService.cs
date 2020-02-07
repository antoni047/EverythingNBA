

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
    using EverythingNBA.Models.MappingTables;

    public class AllStarTeamService : IAllStarTeamService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public AllStarTeamService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
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

        public async Task AddPlayerAsync(int allStarTeamId, int playerId)
        {
            var allStarTeam = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            var obj = new AllStarTeamsPlayers
            {
                AllStarTeamId = allStarTeamId,
                PlayerId = playerId
            };

            allStarTeam.Players.Add(obj);

            await this.db.SaveChangesAsync();
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

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(string type)
        {
            var allStarTeams = await this.db.AllStarTeams.Where(ast => ast.Type.ToString() == type).ToListAsync();

            var allStarTeamServiceModels = new List<GetAllStarTeamServiceModel>();

            foreach (var team in allStarTeams)
            {
                var model = mapper.Map<GetAllStarTeamServiceModel>(team);

                allStarTeamServiceModels.Add(model);
            }

            return allStarTeamServiceModels;
        }

        public async Task<ICollection<GetAllStarTeamServiceModel>> GetAllASTeamsAsync(int Year)
        {
            var allStarTeams = await this.db.AllStarTeams.Where(ast => ast.Year == Year).ToListAsync();

            var allStarTeamServiceModels = new List<GetAllStarTeamServiceModel>();

            foreach (var team in allStarTeams)
            {
                var model = mapper.Map<GetAllStarTeamServiceModel>(team);

                allStarTeamServiceModels.Add(model);
            }

            return allStarTeamServiceModels;
        }

        public async Task<GetAllStarTeamServiceModel> GetAllStarTeamAsync(int id)
        {
            var allStarTeam = await this.db.AllStarTeams.FindAsync(id);

            var model = mapper.Map<GetAllStarTeamServiceModel>(allStarTeam);

            return model;
        }

        public async Task<GetAllStarTeamServiceModel> GetAllStarTeamByTypeAndYearAsync(string type, int Year)
        {
            var allStarTeam = await this.db.AllStarTeams.Where(ast => ast.Type.ToString() == type && ast.Year == Year).FirstOrDefaultAsync();

            var model = mapper.Map<GetAllStarTeamServiceModel>(allStarTeam);

            return model;

        }
    }
}
