using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using EverythingNBA.Models;
using EverythingNBA.Models.Enums;
using EverythingNBA.Data;

namespace EverythingNBA.Services.Implementations
{
    public class PlayerService : IPlayerService
    {
        private readonly EverythingNBADbContext db;
        private readonly IImageService imageService;

        public PlayerService(EverythingNBADbContext db, IImageService imageService)
        {
            this.db = db;
            this.imageService = imageService;
        }

        public async Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, 
            string position, bool isStarter, IFormFile imageFile, int shirtNumber, string instagramLink, string twitterLink, 
            double currentPoints, double currentAssists, double currentRebounds, double currentBlocks, double currentSteals, 
            double currentFreeThrowPercentage, double currentThreePercentage, double currentFieldGoalPercentage)
        {
            var imageId = await this.imageService.UploadImageAsync(imageFile);

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
                CloudinaryImageId = imageId,
                ShirtNumber = shirtNumber,
                InstagramLink = instagramLink,
                TwitterLink = twitterLink,
                CurrentAvereagePoints = currentPoints,
                CurrentAverageBlocks = currentBlocks,
                CurrentAverageAssists = currentAssists,
                CurrentAverageRebounds = currentRebounds,
                CurrentAverageSteals = currentSteals,
                CurrentFieldGoalPercentage = currentFieldGoalPercentage,
                CurrentFreeThrowPercentage = currentFreeThrowPercentage,
                CurrentThreePercentage = currentThreePercentage
            };

            await this.db.Players.AddAsync(playerObj);
            await this.db.SaveChangesAsync();

            return playerObj.Id;

        }
    }
}
