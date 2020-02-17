namespace EverythingNBA.Services.Implementations
{
    using System.Linq;
    using System;
    using AutoMapper;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Models.MappingTables;
    using EverythingNBA.Services.Models.Season;

    public class SeasonService : ISeasonService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeasonService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId, int gamesPlayed)
        {
            var seasonObj = new Season
            {
                Year = year,
                TitleWinnerId = titleWinnerId,
                PlayoffId = playoffId,
                GamesPlayed = gamesPlayed
            };

            this.db.Seasons.Add(seasonObj);

            await this.db.SaveChangesAsync();

            return seasonObj.Id;
        }

        public async Task AddAllStarTeamAsync(int seasonId, int allStarTeamId)
        {
            var season = await this.db.Seasons
                .Include(s => s.AllStarTeams)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var allStarTeam = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            season.AllStarTeams.Add(allStarTeam);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemoveAllStarTeamAsync(int seasonId, int allStarTeamId)
        {
            var season = await this.db.Seasons
                .Include(s => s.AllStarTeams)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            if (season == null)
            {
                return false;
            }

            var allStarTeam = await this.db.AllStarTeams.FindAsync(allStarTeamId);

            if (allStarTeam == null)
            {
                return false;
            }

            season.AllStarTeams.Remove(allStarTeam);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task AddAwardAsync(int seasonId, int awardId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Awards)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var award = await this.db.Awards.FindAsync(awardId);

            season.Awards.Add(award);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemoveAwardAsync(int seasonId, int awardId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Awards)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            if (season == null)
            {
                return false;
            }

            var award = await this.db.Awards.FindAsync(awardId);

            if (award == null)
            {
                return false;
            }

            season.Awards.Remove(award);

            await this.db.SaveChangesAsync();
            return true;
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

        public async Task<bool> RemovePlayoffAsync(int seasonId, int playoffId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Playoff)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            if (season == null || this.db.Playoffs.Find(playoffId) == null)
            {
                return false;
            }

            season.PlayoffId = null;

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task AddSeasonStatisticAsync(int seasonId, int seasonStatisticId)
        {
            var season = await this.db.Seasons
                .Include(s => s.SeasonStatistics)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            season.SeasonStatistics.Add(seasonStatistic);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemoveSeasonStatisticAsync(int seasonId, int seasonStatisticId)
        {
            var season = await this.db.Seasons
                 .Include(s => s.SeasonStatistics)
                 .Where(s => s.Id == seasonId)
                 .FirstOrDefaultAsync();

            if (season == null)
            {
                return false;
            }

            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            if (seasonStatistic == null)
            {
                return false;
            }

            season.SeasonStatistics.Remove(seasonStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task AddGameAsync(int seasonId, int gameId)
        {
            var season = await this.db.Seasons
                .Include(s => s.Games)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var game = await this.db.Games.FindAsync(gameId);

            season.Games.Add(game);

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemoveGameAsync(int seasonId, int gameId)
        {
            var season = await this.db.Seasons
                   .Include(s => s.Games)
                   .Where(s => s.Id == seasonId)
                   .FirstOrDefaultAsync();

            if (season == null)
            {
                return false;
            }

            var game = await this.db.Games.FindAsync(gameId);

            if (game == null)
            {
                return false;
            }

            season.Games.Remove(game);

            await this.db.SaveChangesAsync();
            return true;
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
                .Include(s => s.Awards)
                .Include(s => s.AllStarTeams)
                .Where(s => s.Id == seasonId)
                .FirstOrDefaultAsync();

            var bestSeed = season.SeasonStatistics.OrderByDescending(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();
            var worstSeed = season.SeasonStatistics.OrderBy(s => s.Wins).Select(s => s.Team.Name).FirstOrDefault();

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
            var seasons = await this.db.Seasons.ToListAsync();

            var models = new List<GetSeasonListingServiceModel>();

            foreach (var season in seasons)
            {
                var model = mapper.Map<GetSeasonListingServiceModel>(season);

                models.Add(model);
            }

            return models;
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
