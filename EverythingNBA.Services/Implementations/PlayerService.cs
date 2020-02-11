namespace EverythingNBA.Services.Implementations
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
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

        public async Task<bool> AddAward(int playerId, int awardId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            var award = await this.db.Awards.FindAsync(awardId);

            if (player== null || award == null)
            {
                return false;
            }

            player.Awards.Add(award);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddGameStatistic(int playerId, int gameStatisticId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            var gameStatistic = await this.db.GameStatistics.FindAsync(gameStatisticId);

            if (player == null || gameStatistic == null)
            {
                return false;
            }


            throw new NotImplementedException();
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

        public async Task<bool> AddPlayerSeasonStatistic(int playerId, int seasonStatisticId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            throw new NotImplementedException();
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
            var player = await this.db.Players.FindAsync(id);

            var model = mapper.Map<PlayerDetailsServiceModel>(player);

            model.CurrentTeam = await this.db.Teams.Where(t => t.Id == player.TeamId).Select(t => t.Name).FirstOrDefaultAsync();

            return model;
        }

        public async Task<PlayerDetailsServiceModel> GetPlayerDetailsAsync(string name)
        {
            var playerId = await this.db.Players.Where(p => p.FirstName + " " + p.LastName == name).Select(p => p.Id).FirstOrDefaultAsync();

            var model = await this.GetPlayerDetailsAsync(playerId);

            return model;
        }

        public async Task<bool> RemoveAward(int playerId, int awardId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            var award = await this.db.Awards.FindAsync(awardId);
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveGameStatisticStatistic(int playerId, int gameStatisticId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            throw new NotImplementedException();
        }

        public async Task<bool> RemovePlayerSeasonStatistic(int playerId, int seasonStatisticId)
        {
            var player = await this.db.Players.FindAsync(playerId);
            throw new NotImplementedException();
        }
    }
}
