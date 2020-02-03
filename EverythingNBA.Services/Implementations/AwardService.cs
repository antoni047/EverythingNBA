

namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;

    using System;
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<ICollection<string>> GetAllAwardWinnersAsync(int seasonId)
        {
            var winnersNames = new List<string>();

            var awards = await this.db.Awards.Where(a => a.SeasonId == seasonId).ToListAsync();

            foreach (var award in awards)
            {
                var name = award.Winner.FirstName + " " + award.Winner.LastName;

                winnersNames.Add(name);
            }

            return winnersNames;
        }

        public async Task<string> GetAwardWinnerAsync(int seasonId, string awardType)
        {
            Player player = new Player();
            string name = string.Empty;

            var awards = await this.db.Awards.Where(a => a.SeasonId == seasonId).ToListAsync();

            switch (awardType)
            {
                case "MVP":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;
                case "Top Scorer":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                case "DPOTY":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                case "ROTY":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                case "MIP":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                case "SixthMOTY":
                    player = awards.Where(a => a.Name.ToString() == "MVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                default:
                    return null;
            } //returns name
        }

        public async Task<ICollection<string>> GetPlayerAwardsAsync(int playerId)
        {
            var playerAwards = await this.db.Awards.Where(a => a.WinnerId == playerId).Select(a => a.Name.ToString()).ToListAsync();

            return playerAwards;

            //foreach (var award in awards)
            //{
            //    awardType = award.Name.ToString();
            //}
        }
    }
}
