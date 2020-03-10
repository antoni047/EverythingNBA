namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Linq;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;

    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Data;
    using EverythingNBA.Services.Models.Player;
    using System.Globalization;

    public class PlayerService : IPlayerService
    {
        private readonly EverythingNBADbContext db;
        private readonly IImageService imageService;
        private readonly IMapper mapper;
        private readonly ISeasonService seasonService;
        private readonly IGameService gameService;
        private readonly IAwardService awardService;
        private readonly IAllStarTeamService astService;
        //private readonly IGameStatisticService gameStatsService;

        public PlayerService(EverythingNBADbContext db, IImageService imageService, IMapper mapper, ISeasonService seasonService,
            IGameService gameService, IAwardService awardService, IAllStarTeamService astService/*, IGameStatisticService gameStatsService*/)
        {
            this.db = db;
            this.imageService = imageService;
            this.mapper = mapper;
            this.seasonService = seasonService;
            this.gameService = gameService;
            this.awardService = awardService;
            this.astService = astService;
            //this.gameStatsService = gameStatsService;
        }

        public async Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight,
            string position, bool isStarter, /*IFormFile imageFile,*/ int shirtNumber, string instagramLink, string twitterLink)
        {
            //var imageId = await this.imageService.UploadImageAsync(imageFile);

            var playerObj = new Player
            {
                FirstName = firstName,
                LastName = lastName,
                TeamId = teamId,
                RookieYear = rookieYear,
                Age = age,
                Height = height,
                Weight = weight,
                Position = (PositionType)Enum.Parse(typeof(PositionType), position),
                IsStarter = isStarter,
                //CloudinaryImageId = imageId,
                ShirtNumber = shirtNumber,
                InstagramLink = instagramLink,
                TwitterLink = twitterLink,

            };

            await this.db.Players.AddAsync(playerObj);
            await this.db.SaveChangesAsync();

            return playerObj.Id;

        }

        public async Task<bool> DeletePlayerAsync(int playerId)
        {
            var playerToDelete = await this.db.Players.FindAsync(playerId);

            if (playerToDelete == null)
            {
                return false;
            }

            this.db.Remove(playerToDelete);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<string>> GetAllPlayersAsync()
        {
            var allPlayers = await this.db.Players.ToListAsync();

            var names = new List<string>();

            foreach (var player in allPlayers.OrderBy(p => p.FirstName))
            {
                var name = player.FirstName + " " + player.LastName;

                names.Add(name);
            }

            return names;
        }

        public async Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(int id)
        {
            var player = await this.db.Players
                .Include(p => p.SingleGameStatistics)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            var season = await this.seasonService.GetDetailsByYearAsync(this.GetCurrentSeasonYear());

            var model = mapper.Map<PlayerDetailsServiceModel>(player);

            model.CurrentTeam = await this.db.Teams.Include(t => t.Players).Where(t => t.Id == player.TeamId).Select(t => t.Name).FirstOrDefaultAsync();
            model.SeasonStatistics = await this.GetSeasonStatistics(player.Id, season.SeasonId);
            model.CareerStatistics = await this.GetCareerStatistics(player.Id);
            model.RecentGames = await this.GetRecentGamesAsync(player.Id);

            return model;
        }

        public async Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(string name)
        {
            var playerId = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == name).Select(p => p.Id).FirstOrDefaultAsync();

            var model = await this.GetPlayerDetailsAsync(playerId);

            return model;
        }

        public async Task<bool> AddAward(int playerId, int awardId)
        {
            var player = await this.db.Players
                .Include(p => p.Awards)
                .Where(p => p.Id == playerId)
                .FirstOrDefaultAsync();

            var award = await this.db.Awards.FindAsync(awardId);

            if (player == null || award == null || playerId != award.WinnerId)
            {
                return false;
            }

            player.Awards.Add(award);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveAward(int playerId, int awardId)
        {
            var player = await this.db.Players
                 .Include(p => p.Awards)
                 .Where(p => p.Id == playerId)
                 .FirstOrDefaultAsync();

            var award = await this.db.Awards.FindAsync(awardId);

            if (player == null || award == null || playerId != award.WinnerId)
            {
                return false;
            }

            player.Awards.Remove(award);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddGameStatistic(int playerId, int gameStatisticId)
        {
            var player = await this.db.Players
              .Include(p => p.SingleGameStatistics)
              .Where(p => p.Id == playerId)
              .FirstOrDefaultAsync();

            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            if (player == null || gameStatistic == null || playerId != gameStatistic.PlayerId)
            {
                return false;
            }

            player.SingleGameStatistics.Add(gameStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGameStatisticStatistic(int playerId, int gameStatisticId)
        {
            var player = await this.db.Players
              .Include(p => p.SingleGameStatistics)
              .Where(p => p.Id == playerId)
              .FirstOrDefaultAsync();

            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            if (player == null || gameStatistic == null || playerId != gameStatistic.PlayerId)
            {
                return false;
            }

            player.SingleGameStatistics.Remove(gameStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerSeasonStatisticServiceModel> GetSeasonStatistics(int playerId, int seasonId)
        {
            var player = await this.db.Players
                .Include(p => p.SingleGameStatistics).ThenInclude(gs => gs.Game)
                .Where(p => p.Id == playerId)
                .FirstOrDefaultAsync();

            var seasonGameStatistics = player.SingleGameStatistics.Where(gs => gs.Game.SeasonId == seasonId).ToList();
            var totalGames = seasonGameStatistics.Count();

            var points = new List<double>();
            var assists = new List<double>();
            var rebounds = new List<double>();
            var blocks = new List<double>();
            var steals = new List<double>();
            var freeThrowsMade = new List<double>();
            var freeThrowsAttempted = new List<double>();
            var fieldGoalsMade = new List<double>();
            var fieldGoalsAttempted = new List<double>();
            var threesMade = new List<double>();
            var threesAttempted = new List<double>();

            foreach (var gameStat in seasonGameStatistics)
            {
                points.Add(gameStat.Points);
                assists.Add(gameStat.Assists);
                rebounds.Add(gameStat.Rebounds);
                steals.Add(gameStat.Steals);
                blocks.Add(gameStat.Blocks);
                freeThrowsMade.Add(gameStat.FreeThrowsMade);
                freeThrowsAttempted.Add(gameStat.FreeThrowAttempts);
                fieldGoalsMade.Add(gameStat.FieldGoalsMade);
                fieldGoalsAttempted.Add(gameStat.FieldGoalAttempts);
                threesMade.Add(gameStat.ThreeMade);
                threesAttempted.Add(gameStat.ThreeAttempts);
            }

            double averagePoints = points.Any() ? points.Average() : 0;
            double averageAssists = assists.Any() ? assists.Average() : 0;
            double averageRebounds = rebounds.Any() ? rebounds.Average() : 0;
            double averageSteals = steals.Any() ? steals.Average() : 0;
            double averageBlocks = blocks.Any() ? blocks.Average() : 0;

            double averageThreePercentage = threesAttempted.Any() && threesMade.Any() 
                ? threesMade.Average() != 0 && threesAttempted.Average() != 0 
                ? (threesMade.Average() / threesAttempted.Average()) * 100 : 0 : 0;

            double averageFreeThrowPercentage = freeThrowsMade.Any() && freeThrowsAttempted.Any()
                ? freeThrowsMade.Average() != 0 && freeThrowsAttempted.Average() != 0
                ? (freeThrowsMade.Average() / freeThrowsAttempted.Average()) * 100 : 0 :0;

            double averageFieldGoalPercentage = fieldGoalsMade.Any() && fieldGoalsAttempted.Any() 
                ? fieldGoalsMade.Average() != 0 && fieldGoalsAttempted.Average() != 0
                ? (fieldGoalsMade.Average() / fieldGoalsAttempted.Average()) * 100 : 0 : 0;

            var seasonStatModel = new PlayerSeasonStatisticServiceModel
            {
                PlayerId = playerId,
                SeasonId = seasonId,
                AveragePoints = Math.Round(averagePoints, 1, MidpointRounding.AwayFromZero),
                AverageAssists = Math.Round(averageAssists, 1, MidpointRounding.AwayFromZero),
                AverageRebounds = Math.Round(averageRebounds, 1, MidpointRounding.AwayFromZero),
                AverageBlocks = Math.Round(averageBlocks, 1, MidpointRounding.AwayFromZero),
                AverageSteals = Math.Round(averageSteals, 1, MidpointRounding.AwayFromZero),
                AverageFreeThrowPercentage = Math.Round(averageFreeThrowPercentage, 1, MidpointRounding.AwayFromZero),
                AverageFieldGoalPercentage = Math.Round(averageFieldGoalPercentage, 1, MidpointRounding.AwayFromZero),
                AverageThreePercentage = Math.Round(averageThreePercentage, 1, MidpointRounding.AwayFromZero)
            };

            return seasonStatModel;
        }

        public async Task<PlayerCareerStatisticServiceModel> GetCareerStatistics(int playerId)
        {
            var seasonIds = await this.db.Seasons.Select(s => s.Id).ToListAsync();

            double totalPoints = 0;
            double totalAssists = 0;
            double totalRebounds = 0;
            double totalSteals = 0;
            double totalBlocks = 0;
            double totalFGPercentage = 0;
            double totalFTPercentage = 0;
            double totalThreePercentage = 0;
            var gamesPlayed = 0;

            foreach (var seasonId in seasonIds)
            {
                var seasonStats = await this.GetSeasonStatistics(playerId, seasonId);

                gamesPlayed = await this.db.Players
                    .Where(p => p.Id == playerId)
                    .Select(p => p.SingleGameStatistics.Count())
                    .FirstOrDefaultAsync();

                if (seasonStats != null)
                {
                    totalPoints += seasonStats.AveragePoints;
                    totalAssists += seasonStats.AverageAssists;
                    totalRebounds += seasonStats.AverageRebounds;
                    totalSteals += seasonStats.AverageSteals;
                    totalBlocks += seasonStats.AverageBlocks;
                    totalFGPercentage += seasonStats.AverageFieldGoalPercentage;
                    totalFTPercentage += seasonStats.AverageFreeThrowPercentage;
                    totalThreePercentage += seasonStats.AverageThreePercentage;
                }
            }

            var model = new PlayerCareerStatisticServiceModel
            {
                AveragePoints = totalPoints != 0 ? Math.Round(totalPoints / gamesPlayed, 0) : 0,
                AverageAssists = totalAssists != 0 ? Math.Round(totalAssists / gamesPlayed, 0): 0,
                AverageRebounds = totalRebounds != 0 ? Math.Round(totalRebounds / gamesPlayed, 0) : 0,
                AverageSteals = totalSteals != 0 ? Math.Round(totalSteals / gamesPlayed, 0) : 0,
                AverageBlocks = totalBlocks != 0 ? Math.Round(totalBlocks / gamesPlayed, 0) : 0,
                AverageFieldGoalPercentage = totalFGPercentage  != 0 ? Math.Round(totalFGPercentage / gamesPlayed, 0) : 0,
                AverageFreeThrowPercentage = totalFTPercentage != 0 ? Math.Round(totalFTPercentage / gamesPlayed, 0) : 0,
                AverageThreePercentage = totalThreePercentage != 0 ? Math.Round(totalThreePercentage / gamesPlayed, 0) : 0
            };

            return model;
        }

        public async Task<ICollection<PlayerRecentGamesListingServiceModel>> GetRecentGamesAsync(int playerId)
        {
            var player = await this.db.Players
                .Where(p => p.Id == playerId)
                .Include(p => p.SingleGameStatistics)
                    .ThenInclude(gs => gs.Game)
                        .ThenInclude(g => g.Team2)
                .Include(p => p.SingleGameStatistics)
                    .ThenInclude(gs => gs.Game)
                        .ThenInclude(g => g.TeamHost)
                .FirstOrDefaultAsync();

            var gameStats = player.SingleGameStatistics.OrderByDescending(gs => gs.Game.Date).ToList().Take(9).ToList();

            var modelsList = new List<PlayerRecentGamesListingServiceModel>();

            foreach (var gameStat in gameStats)
            {
                var model = new PlayerRecentGamesListingServiceModel
                {
                    TeamHostName = gameStat.Game.TeamHost.Name,
                    Team2Name = gameStat.Game.Team2.Name,
                    TeamHostPoints = (int)gameStat.Game.TeamHostPoints,
                    Team2Points = (int)gameStat.Game.Team2Points,
                    Date = gameStat.Game.Date.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
                    Points = (int)gameStat.Points,
                    Assists = (int)gameStat.Assists,
                    Rebounds = (int)gameStat.Rebounds
                };

                modelsList.Add(model);
            }

            return modelsList;
        }

        public async Task EditPlayerAsync(PlayerDetailsServiceModel model, int id)
        {
            var player = await this.db.Players.FindAsync(id);

            var playerTeam = await this.db.Teams.Where(t => t.Name == model.CurrentTeam).FirstOrDefaultAsync();

            player.FirstName = model.FirstName;
            player.LastName = model.LastName;
            player.IsStarter = model.IsStarter;
            player.Height = model.Height;
            player.Weight = model.Weight;
            player.Age = model.Age;
            player.InstagramLink = model.InstagramLink;
            player.TwitterLink = model.TwitterLink;
            player.Position = (PositionType)Enum.Parse(typeof(PositionType), model.Position);
            player.TeamId = playerTeam.Id;
            player.ShirtNumber = model.ShirtNumber;
            player.RookieYear = model.RookieYear;
            player.CloudinaryImageId = model.CloudinaryImageId;

            await this.db.SaveChangesAsync();
        }

        public async Task<PlayerAccomplishmentsListingServiceModel> GetPlayerAccomplishentsAsync(int playerId)
        {
            var player = await this.db.Players
                .Where(p => p.Id == playerId)
                .Include(p => p.AllStarTeams)
                    .ThenInclude(astp => astp.AllStarTeam)
                .Include(p => p.Awards)
                .FirstOrDefaultAsync();

            var firstNBATeams = new List<int>();
            var secondNBATeams = new List<int>();
            var thirdNBATeams = new List<int>();
            var allDefensiveTeams = new List<int>();
            var allRookieTeams = new List<int>();

            var MVPs = new List<int>();
            var FinalsMVPs = new List<int>();
            var TopScorerTitles = new List<int>();
            var DPOTYs = new List<int>();
            var ROTYs = new List<int>();
            var SixthMOTYs = new List<int>();
            var MIPs = new List<int>();

            foreach (var team in player.AllStarTeams)
            {
                if (team.AllStarTeam.Type.ToString() == "FirstAllNBA")
                {
                    firstNBATeams.Add(team.AllStarTeam.Year);
                }

                else if (team.AllStarTeam.Type.ToString() == "SecondAllNBA")
                {
                    secondNBATeams.Add(team.AllStarTeam.Year);
                }

                else if (team.AllStarTeam.Type.ToString() == "ThirdAllNBA")
                {
                    thirdNBATeams.Add(team.AllStarTeam.Year);
                }

                else if (team.AllStarTeam.Type.ToString() == "AllDefensive")
                {
                    allDefensiveTeams.Add(team.AllStarTeam.Year);
                }

                else if (team.AllStarTeam.Type.ToString() == "AllRookie")
                {
                    allRookieTeams.Add(team.AllStarTeam.Year);
                }
            }

            foreach (var award in player.Awards)
            {
                if (award.Name.ToString() == "MVP")
                {
                    MVPs.Add(award.Year);
                }

                else if (award.Name.ToString() == "FinalsMVP")
                {
                    FinalsMVPs.Add(award.Year);
                }

                else if (award.Name.ToString() == "ROTY")
                {
                    ROTYs.Add(award.Year);
                }

                else if (award.Name.ToString() == "TopScorer")
                {
                    TopScorerTitles.Add(award.Year);
                }

                else if (award.Name.ToString() == "DPOTY")
                {
                    DPOTYs.Add(award.Year);
                }

                else if (award.Name.ToString() == "SixthMOTY")
                {
                    SixthMOTYs.Add(award.Year);
                }

                else if (award.Name.ToString() == "MIP")
                {
                    MIPs.Add(award.Year);
                }
            }


            var model = new PlayerAccomplishmentsListingServiceModel
            {
                MVPs = MVPs,
                FinalsMVPs = FinalsMVPs,
                TopScorerTitles = TopScorerTitles,
                DPOTYs = DPOTYs,
                ROTYs = ROTYs,
                SixthMOTYs = SixthMOTYs,
                MIPs = MIPs
            };

            return model;
        }

        public async Task<ICollection<TeamPlayerOverviewServiceModel>> GetAllPlayersFromTeam(int teamId)
        {
            var players = await this.db.Players.Where(p => p.TeamId == teamId).ToListAsync();

            var playerModels = new List<TeamPlayerOverviewServiceModel>();

            foreach (var player in players.OrderByDescending(p => p.IsStarter).ToList())
            {
                var model = mapper.Map<TeamPlayerOverviewServiceModel>(player);

                playerModels.Add(model);
            }

            return playerModels;
        }

        public async Task<bool> AddAllStarTeam(string playerName, int allStarTeamId)
        {
            var obj = await this.db.AllStarTeamsPlayers
                .Include(x => x.Player)
                    .ThenInclude(p => p.AllStarTeams)
                .Include(x => x.AllStarTeam)
                    .ThenInclude(ast => ast.Players)
                .Where(x => x.AllStarTeamId == allStarTeamId && x.Player.FirstName + " " + x.Player.LastName == playerName)
                .FirstOrDefaultAsync();

            if (obj == null || obj.Player == null)
            {
                return false;
            }

            obj.Player.AllStarTeams.Add(obj);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveAllStarTeam(string playerName, int allStarTeamId)
        {
           var obj = await this.db.AllStarTeamsPlayers
                .Include(x => x.Player)
                    .ThenInclude(p => p.AllStarTeams)
                .Include(x => x.AllStarTeam)
                    .ThenInclude(ast => ast.Players)
                .Where(x => x.AllStarTeamId == allStarTeamId && x.Player.FirstName + " " + x.Player.LastName == playerName)
                .FirstOrDefaultAsync();

            if (obj == null || obj.Player == null)
            {
                return false;
            }

            obj.Player.AllStarTeams.Remove(obj);
            obj.Player = null;

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddPlayerToTeamAsync(int teamId, int playerId)
        {
            var player = await this.db.Players.Include(p => p.Team).Where(p => p.Id == playerId).FirstOrDefaultAsync();
            var team = await this.db.Teams.Where(t => t.Id == teamId).FirstOrDefaultAsync();

            if (player == null || team == null)
            {
                return false;
            }

            player.Team = team;
            team.Players.Add(player);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemovePlayerFromTeamAsync(int teamId, int playerId)
        {
            var player = await this.db.Players.Include(p => p.Team).Where(p => p.Id == playerId).FirstOrDefaultAsync();
            var team = await this.db.Teams.Where(t => t.Id == teamId).FirstOrDefaultAsync();

            if (player == null || team == null)
            {
                return false;
            }

            player.Team = null;
            team.Players.Remove(player);
            await this.db.SaveChangesAsync();

            return true;
        }

        private int GetCurrentSeasonYear()
        {
            var currentYear = 0;

            if (DateTime.Now.Month >= 9)
            {
                currentYear = DateTime.Now.Year + 1;
            }
            else if (DateTime.Now.Month < 9)
            {
                currentYear = DateTime.Now.Year;
            }

            return currentYear;
        }
    }
}
