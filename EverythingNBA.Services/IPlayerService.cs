using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EverythingNBA.Services
{
    public interface IPlayerService
    {
        Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, string position,
            bool isStarter, IFormFile pictureFile, int shirtNumber, string instagramLink, string twitterLink, double currentPoints,
            double currentAssists, double currentRebounds, double currentBlocks, double currentSteals, double currentFreeThrowPercentage,
            double currentThreePercentage, double currentFieldGoalPercentage);
    }
}
