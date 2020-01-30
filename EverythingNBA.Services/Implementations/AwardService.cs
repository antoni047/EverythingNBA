

namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;

    using System;
    using System.Threading.Tasks;

    public class AwardService : IAwardService
    {
        private readonly EverythingNBADbContext db;

        public AwardService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAwardAsync(string name, int year, int winnerId)
        {
            var awardObj = new Award
            {
                Name = (AwardType)Enum.Parse(typeof(AwardType), name),
                Year = year,
                WinnerId = winnerId
            };

            this.db.Awards.Add(awardObj);
            await this.db.SaveChangesAsync();

            return awardObj.Id;
        }

        public async Task<bool> DeleteAwardAsync(int awardId)
        {
            var awardtoDelete = await this.db.Awards.FindAsync(awardId);

            if (awardtoDelete == null)
            {
                return false;
            }

            this.db.Remove(awardtoDelete);
            await this.db.SaveChangesAsync();

            return true;
        }
    }
}
