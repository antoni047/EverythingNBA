namespace EverythingNBA.Services.Implementations
{
    using System.Linq;
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using AutoMapper;
    using System.Collections.Generic;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Models.MappingTables;

    public class SeasonService : ISeasonService
    {
        private readonly EverythingNBADbContext db;

        public SeasonService(EverythingNBADbContext db)
        {
            this.db = db;
        }

        public async Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId, int gamesPlayed)
        {
            var seasonObj = new Season
            {
                Year = year,
                TitleWinnerId = titleWinnerId,
                PlayoffId = playoffId
            };

            this.db.Seasons.Add(seasonObj);

            await this.db.SaveChangesAsync();

            return seasonObj.Id;
        }

        public async Task AddAllStarTeamAsync(int seasonId, int allStarTeamId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            var allStarTeam = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            season.AllStarTeams.Add(allStarTeam);

            await this.db.SaveChangesAsync();
        }

        public async Task AddAwardAsync(int seasonId, int awardId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            var award = await this.db.Awards.FindAsync(awardId);

            season.Awards.Add(award);

            await this.db.SaveChangesAsync();
        }

        public async Task AddPlayoffAsync(int seasonId, int playoffId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            season.PlayoffId = playoffId;

            await this.db.SaveChangesAsync();
        }

        public async Task AddSeasonStatisticAsync(int seasonId, int seasonStatisticId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            var seasonStatistic = await this.db.SingleSeasonStatistics.FindAsync(seasonStatisticId);

            season.SingleSeasonStatistics.Add(seasonStatistic);

            await this.db.SaveChangesAsync();
        }

        public async Task AddGameAsync(int seasonId, int gameId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            var game = await this.db.Games.FindAsync(gameId);

            season.Games.Add(game);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int seasonId)
        {
            var seasonToDelete = await this.db.Seasons.FindAsync(seasonId);

            if (seasonToDelete == null)
            {
                return false;
            }

            this.db.Seasons.Remove(seasonToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<GetSeasonDetailsServiceModel> GetSeasonByIdAsync(int seasonId)
        {
            var season = await this.db.Seasons.FindAsync(seasonId);

            var bestSeed = season.SingleSeasonStatistics.OrderByDescending(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();
            var worstSeed = season.SingleSeasonStatistics.OrderBy(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();

            var awardWinners = this.GetSeasonAwards(season);
            var allStarTeams = this.GetAllStarTeams(season);

            var model = new GetSeasonDetailsServiceModel
            {
                Year = season.Year,
                TitleWinnerId = season.TitleWinnerId,
                GamesPlayed = season.GamesPlayed,
                PlayoffId = season.PlayoffId,
                BestSeed = bestSeed,
                WorstSeed = worstSeed,
                MVP = awardWinners[0],
                TopScorer = awardWinners[1],
                DPOTY = awardWinners[2],
                SixthMOTY = awardWinners[3],
                ROTY = awardWinners[4],
                MIP = awardWinners[5],
                FinalsMVP = awardWinners[6],
                FirstAllNBATeamId = allStarTeams[0].Id,
                SecondAllNBATeamId = allStarTeams[1].Id,
                ThirdAllNBATeamId = allStarTeams[2].Id,
                AllDefenesiveTeamId = allStarTeams[3].Id,
                AllRookieTeamId = allStarTeams[4].Id
            };

            return model;
        }

        public async Task<GetSeasonDetailsServiceModel> GetSeasonByYearAsync(int Year)
        {
            var season = await this.db.Seasons.Where(s => s.Year == Year).FirstOrDefaultAsync();

            var bestSeed = season.SingleSeasonStatistics.OrderByDescending(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();
            var worstSeed = season.SingleSeasonStatistics.OrderBy(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();

            var awardWinners = this.GetSeasonAwards(season);
            var allStarTeams = this.GetAllStarTeams(season);

            var model = new GetSeasonDetailsServiceModel
            {
                Year = season.Year,
                TitleWinnerId = season.TitleWinnerId,
                GamesPlayed = season.GamesPlayed,
                PlayoffId = season.PlayoffId,
                BestSeed = bestSeed,
                WorstSeed = worstSeed,
                MVP = awardWinners[0],
                TopScorer = awardWinners[1],
                DPOTY = awardWinners[2],
                SixthMOTY = awardWinners[3],
                ROTY = awardWinners[4],
                MIP = awardWinners[5],
                FinalsMVP = awardWinners[6],
                FirstAllNBATeamId = allStarTeams[0].Id,
                SecondAllNBATeamId = allStarTeams[1].Id,
                ThirdAllNBATeamId = allStarTeams[2].Id,
                AllDefenesiveTeamId = allStarTeams[3].Id,
                AllRookieTeamId = allStarTeams[4].Id
            };

            return model;
        }

        private List<AllStarTeam> GetAllStarTeams(Season season)
        {
            var firstAllNBATeam = season.AllStarTeams.Where(ast => ast.Type.ToString() == "FirstAllNBA").FirstOrDefault();
            var secondAllNBATeam = season.AllStarTeams.Where(ast => ast.Type.ToString() == "SecondAllNBA").FirstOrDefault();
            var thirdAllNBATeam = season.AllStarTeams.Where(ast => ast.Type.ToString() == "ThirdAllNBA").FirstOrDefault();
            var allDefensiveTeam = season.AllStarTeams.Where(ast => ast.Type.ToString() == "AllDefensive").FirstOrDefault();
            var allRookieTeam = season.AllStarTeams.Where(ast => ast.Type.ToString() == "AllRookie").FirstOrDefault();

            var list = new List<AllStarTeam>
            {
                firstAllNBATeam,
                secondAllNBATeam,
                thirdAllNBATeam,
                allDefensiveTeam,
                allRookieTeam
            };

            return list;
        }

        private List<string> GetSeasonAwards(Season season)
        {
            var MVP = season.Awards.Where(a => a.Name.ToString() == "MVP").FirstOrDefault();
            var MVPName = MVP.Winner.FirstName + " " + MVP.Winner.LastName;

            var topScorer = season.Awards.Where(a => a.Name.ToString() == "Top Scorer").FirstOrDefault();
            var topScorerName = topScorer.Winner.FirstName + " " + topScorer.Winner.LastName;

            var DPOTY = season.Awards.Where(a => a.Name.ToString() == "DPOTY").FirstOrDefault();
            var DPOTYName = DPOTY.Winner.FirstName + " " + DPOTY.Winner.LastName;

            var sixthMOTY = season.Awards.Where(a => a.Name.ToString() == "SixthMOTY").FirstOrDefault();
            var sixthMOTYName = sixthMOTY.Winner.FirstName + " " + sixthMOTY.Winner.LastName;

            var ROTY = season.Awards.Where(a => a.Name.ToString() == "ROTY").FirstOrDefault();
            var ROTYName = ROTY.Winner.FirstName + " " + ROTY.Winner.LastName;

            var MIP = season.Awards.Where(a => a.Name.ToString() == "MIP").FirstOrDefault();
            var MIPName = MIP.Winner.FirstName + " " + MIP.Winner.LastName;

            var finalsMVP = season.Awards.Where(a => a.Name.ToString() == "FinalsMVP").FirstOrDefault();
            var finalsMVPName = finalsMVP.Winner.FirstName + " " + finalsMVP.Winner.LastName;

            var list = new List<string>
            {
                MVPName,
                topScorerName,
                DPOTYName,
                sixthMOTYName,
                ROTYName,
                MIPName,
                finalsMVPName
            };

            return list;
        }
    }
}
