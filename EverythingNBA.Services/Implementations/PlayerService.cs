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

    public class PlayerService : IPlayerService
    {
        private readonly EverythingNBADbContext db;
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public PlayerService(EverythingNBADbContext db, IImageService imageService, IMapper mapper)
        {
            this.db = db;
            this.imageService = imageService;
            this.mapper = mapper;
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
                .Include(p => p.Awards)
                .Include(p => p.AllStarTeams)
                .Include(p => p.SingleGameStatistics)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            var model = mapper.Map<PlayerDetailsServiceModel>(player);

            model.CurrentTeam = await this.db.Teams.Include(t => t.Players).Where(t => t.Id == player.TeamId).Select(t => t.Name).FirstOrDefaultAsync();

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

            var averagePoints = points.Average();
            var averageAssists = assists.Average();
            var averageRebounds = rebounds.Average();
            var averageSteals = steals.Average();
            var averageBlocks = blocks.Average();
            var averageFreeThrowPercentage = (freeThrowsMade.Average() / freeThrowsAttempted.Average()) * 100;
            var averageFieldGoalPercentage = (fieldGoalsMade.Average() / fieldGoalsAttempted.Average()) * 100;
            var averageThreePercentage = (threesMade.Average() / threesAttempted.Average()) * 100;

            var seasonStatModel = new PlayerSeasonStatisticServiceModel
            {
                PlayerId = playerId,
                SeasonId = seasonId,
                AveragePoints = averagePoints.ToString("0.0"),
                AverageAssists = averageAssists.ToString("0.0"),
                AverageRebounds = averageRebounds.ToString("0.0"),
                AverageBlocks = averageBlocks.ToString("0.0"),
                AverageSteals = averageSteals.ToString("0.0"),
                AverageFreeThrowPercentage = averageFreeThrowPercentage,
                AverageFieldGoalPercentage = averageFieldGoalPercentage,
                AverageThreePercentage = averageThreePercentage
            };

            return seasonStatModel;
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
    }
}
