﻿

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
    using EverythingNBA.Services.Models.Award;

    public class AwardService : IAwardService
    {
        private readonly EverythingNBADbContext db;

        public AwardService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAwardAsync(string name, int year, string winnerName, string winnerTeamName)
        {
            var season = await this.db.Seasons.Include(s => s.Awards).Where(s => s.Year == year).FirstOrDefaultAsync();
            var player = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == winnerName).FirstOrDefaultAsync();

            var awardObj = new Award
            {
                Name = (AwardType)Enum.Parse(typeof(AwardType), name),
                Year = year,
                WinnerId = player.Id,
                SeasonId = season.Id
                //WinnerTeamName = winnerTeamName
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

        public async Task EditAwardWinnerAsync(string winnerName, int awardId)
        {
            var award = await this.db.Awards.FindAsync(awardId);

            var winner = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == winnerName).FirstOrDefaultAsync();

            award.WinnerId = winner.Id;

            await this.db.SaveChangesAsync();
        }

        public async Task<ICollection<AwardDetailsServiceModel>> GetSeasonAwardsAsync(int seasonId)
        {
            var winnersNames = new List<string>();

            var awards = await this.db.Awards
                .Include(a => a.Winner).ThenInclude(p => p.Team)
                .Where(a => a.SeasonId == seasonId)
                .ToListAsync();

            var seasonAwards = new List<AwardDetailsServiceModel>();

            foreach (var award in awards)
            {
                var model = new AwardDetailsServiceModel
                {
                    Winner = award.Winner.FirstName + " " + award.Winner.LastName,
                    Type = award.Name.ToString(),
                    WinnerTeam = award.Winner.Team.Name
                };

                seasonAwards.Add(model);
            }

            return seasonAwards;
        }

        public async Task<string> GetAwardWinnerAsync(int seasonId, string awardType)
        {
            Player player = new Player();
            string name = string.Empty;

            var awards = await this.db.Awards
                .Include(a => a.Winner)
                .Where(a => a.SeasonId == seasonId)
                .ToListAsync();

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

                case "FinalsMVP":
                    player = awards.Where(a => a.Name.ToString() == "FinalsMVP").Select(a => a.Winner).FirstOrDefault();

                    name = player.FirstName + " " + player.LastName;

                    return name;

                default:
                    return null;
            } //returns name
        }

        public async Task<ICollection<PlayerAwardsServiceModel>> GetPlayerAwardsAsync(int playerId)
        {
            var playerAwards = await this.db.Awards.Where(a => a.WinnerId == playerId).ToListAsync();

            var modelsList = new List<PlayerAwardsServiceModel>();

            foreach (var award in playerAwards)
            {
                var model = new PlayerAwardsServiceModel
                {
                    AwardType = award.Name.ToString(),
                    Year = award.Year
                };

                modelsList.Add(model);
            }

            return modelsList;

            //foreach (var award in awards)
            //{
            //    awardType = award.Name.ToString();
            //}
        }

        public ICollection<AllAwardsServiceModel> GetAllAwards()
        {
            var awards = this.db.Awards.ToList();

            var allAwardsModels = new List<AllAwardsServiceModel>();

            foreach (var award in awards)
            {
                var model = new AllAwardsServiceModel();

                if (award.Name.ToString() == "MVP")
                {
                    model.MVP = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "FinalsMVP")
                {
                    model.FinalsMVP = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "MIP")
                {
                    model.MIP = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "DPOTY")
                {
                    model.DPOTY = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "ROTY")
                {
                    model.ROTY = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "TopScorer")
                {
                    model.TopScorer = award.Winner.FirstName + " " + award.Winner.LastName;
                }
                else if (award.Name.ToString() == "SixthMOTY")
                {
                    model.SixthMOTY = award.Winner.FirstName + " " + award.Winner.LastName;
                }

                allAwardsModels.Add(model);
            }

            return allAwardsModels;
        }

        public async Task<AwardDetailsServiceModel> GetAwardDetails(int awardId)
        {
            var award = await this.db.Awards.Include(a => a.Winner).Where(a => a.Id == awardId).FirstOrDefaultAsync();

            var model = new AwardDetailsServiceModel
            {
                Winner = award.Winner.FirstName + " " + award.Winner.LastName,
                //WinnerTeam = award.WinnerTeamName
                Year = award.Year,
                Type = award.Name.ToString()
            };

            return model;
        }
    }
}
