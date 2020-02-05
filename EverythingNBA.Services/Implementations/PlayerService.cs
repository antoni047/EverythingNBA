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

        public PlayerService(EverythingNBADbContext db, IImageService imageService)
        {
            this.db = db;
            this.imageService = imageService;
        }

        public async Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, 
            string position, bool isStarter, IFormFile imageFile, int shirtNumber, string instagramLink, string twitterLink)
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
    }
}
