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
        //private readonly IPictureService pictureService;

        public PlayerService(EverythingNBADbContext db/*, IPictureService pictureService*/)
        {
            this.db = db;
            //this.pictureService = pictureService;
        }

        public async Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, 
            string position, bool isStarter, IFormFile pictureFile, int shirtNumber, string instagramLink, string twitterLink, 
            double currentPoints, double currentAssists, double currentRebounds, double currentBlocks, double currentSteals, 
            double currentFreeThrowPercentage, double currentThreePercentage, double currentFieldGoalPercentage)
        {
            //var picId = await this.pictureService.UploadImageAsync(pictureFile);

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
                //CloudinaryImageId = picId,
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
