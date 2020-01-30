

namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;

    using EverythingNBA.Models;
    using EverythingNBA.Data;
    using EverythingNBA.Models.Enums;

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
    }
}
