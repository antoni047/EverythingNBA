﻿namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using EverythingNBA.Models;
    using Data;
    using Services.Models.Season;

    public class SeasonService : ISeasonService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeasonService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddAsync(int year, string titleWinner, int gamesPlayed)
        {
            var team = await this.db.Teams.Where(t => t.Name == titleWinner).FirstOrDefaultAsync();

            var seasonObj = new Season();

            if (titleWinner == null)
            {
                seasonObj.Year = year;
                seasonObj.GamesPlayed = gamesPlayed;
            }
            else
            {
                seasonObj.Year = year;
                seasonObj.TitleWinner = team;
                seasonObj.GamesPlayed = gamesPlayed;
            }

            this.db.Seasons.Add(seasonObj);

            await this.db.SaveChangesAsync();

            return seasonObj.Id;
        }

        public async Task AddPlayoffAsync(int seasonId, int playoffId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Playoff)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            season.PlayoffId = playoffId;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int seasonId)
        {
            var seasonToDelete = await this.db.Seasons
                .Include(s => s.SeasonStatistics)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var seasonStatistic = await this.db.SeasonStatistics.Where(s => s.SeasonId == seasonToDelete.Id).FirstOrDefaultAsync();

            if (seasonToDelete == null)
            {
                return false;
            }

            if (seasonStatistic != null)
            {
                seasonToDelete.SeasonStatistics.Remove(seasonStatistic);
            }

            this.db.Seasons.Remove(seasonToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<GetSeasonDetailsServiceModel> GetDetailsAsync(int seasonId)
        {
            var season = await this.db.Seasons
                .Include(s => s.SeasonStatistics)
                    .ThenInclude(ss => ss.Team)
                .Include(s => s.Awards)
                    .ThenInclude(a => a.Winner)
                .Include(s => s.AllStarTeams)
                    .ThenInclude(ast => ast.Players)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var bestSeed = season.SeasonStatistics.OrderByDescending(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();
            var worstSeed = season.SeasonStatistics.OrderBy(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();

            var awardWinners = await this.GetSeasonAwards(season.Id);
            var allStarTeams = await this.GetAllStarTeams(season.Id);

            var model = new GetSeasonDetailsServiceModel
            {
                Year = season.Year,
                TitleWinnerId = season.TitleWinnerId,
                GamesPlayed = season.GamesPlayed,
                PlayoffId = season.PlayoffId,
                SeasonId = season.Id,
            };

            return model;
        }

        public async Task<GetSeasonDetailsServiceModel> GetDetailsByYearAsync(int year)
        {
            var seasonId = await this.db.Seasons
                .Where(s => s.Year == year)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            return await this.GetDetailsAsync(seasonId);
        }

        public async Task<ICollection<GetSeasonListingServiceModel>> GetAllSeasonsAsync()
        {
            var seasons = await this.db.Seasons.Include(s => s.TitleWinner).OrderByDescending(s => s.Year).ToListAsync();

            var models = new List<GetSeasonListingServiceModel>();

            foreach (var season in seasons)
            {
                var model = mapper.Map<GetSeasonListingServiceModel>(season);
                if (season.TitleWinner == null)
                {
                    model.TitleWinnerName = "N/A";
                }
                else
                {
                    model.TitleWinnerName = season.TitleWinner.Name;
                }

                models.Add(model);
            }

            return models;
        }

        public async Task EditSeasonAsync(GetSeasonDetailsServiceModel model, int seasonId)
        {
            if (model == null) { return; }

            var season = await this.db.Seasons.FindAsync(seasonId);

            season.Year = model.Year;
            season.TitleWinnerId = model.TitleWinnerId;
            season.GamesPlayed = model.GamesPlayed;

            await this.db.SaveChangesAsync();
        }

        public async Task<int> GetYearAsync(int seasonId)
        {
            return await this.db.Seasons.Where(s => s.Id == seasonId).Select(s => s.Year).FirstOrDefaultAsync();
        }

        public async Task<DateTime> GetSeasonStartDateAsync(int seasonId)
        {
            return await this.db.Seasons.Where(s => s.Id == seasonId).Select(s => s.SeasonStartDate).FirstOrDefaultAsync();
        }

        public async Task<DateTime> GetSeasonEndDateAsync(int seasonId)
        {
            return await this.db.Seasons.Where(s => s.Id == seasonId).Select(s => s.SeasonEndDate).FirstOrDefaultAsync();
        }

        public async Task CreateInitialSeasonStatistics(int seasonId)
        {
            var teams = await this.db.Teams.ToListAsync();

            foreach (var team in teams)
            {
                var statistic = new SeasonStatistic
                {
                    SeasonId = seasonId,
                    Team = team,
                    Wins = 0,
                    Losses = 0,
                };

                this.db.SeasonStatistics.Add(statistic);
            }

            await this.db.SaveChangesAsync();
        }

        private async Task<List<AllStarTeam>> GetAllStarTeams(int seasonId)
        {
            var season = await this.db.Seasons
                .Include(s => s.AllStarTeams)
                    .ThenInclude(ast => ast.Players)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

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

        private async Task<List<string>> GetSeasonAwards(int seasonId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Awards)
                    .ThenInclude(a => a.Winner)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var MVP = season.Awards.Where(a => a.Name.ToString() == "MVP").FirstOrDefault();
            var MVPName = MVP != null ? MVP.Winner.FirstName + " " + MVP.Winner.LastName : "N/A";

            var topScorer = season.Awards.Where(a => a.Name.ToString() == "Top Scorer").FirstOrDefault();
            var topScorerName = topScorer != null ? topScorer.Winner.FirstName + " " + topScorer.Winner.LastName : "N/A";

            var DPOTY = season.Awards.Where(a => a.Name.ToString() == "DPOTY").FirstOrDefault();
            var DPOTYName = DPOTY != null ? DPOTY.Winner.FirstName + " " + DPOTY.Winner.LastName : "N/A";

            var sixthMOTY = season.Awards.Where(a => a.Name.ToString() == "SixthMOTY").FirstOrDefault();
            var sixthMOTYName = sixthMOTY != null ? sixthMOTY.Winner.FirstName + " " + sixthMOTY.Winner.LastName : "N/A";

            var ROTY = season.Awards.Where(a => a.Name.ToString() == "ROTY").FirstOrDefault();
            var ROTYName = ROTY != null ? ROTY.Winner.FirstName + " " + ROTY.Winner.LastName : "N/A";

            var MIP = season.Awards.Where(a => a.Name.ToString() == "MIP").FirstOrDefault();
            var MIPName = MIP != null ? MIP.Winner.FirstName + " " + MIP.Winner.LastName : "N/A";

            var finalsMVP = season.Awards.Where(a => a.Name.ToString() == "FinalsMVP").FirstOrDefault();
            var finalsMVPName = finalsMVP != null ? finalsMVP.Winner.FirstName + " " + finalsMVP.Winner.LastName : "N/A";

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