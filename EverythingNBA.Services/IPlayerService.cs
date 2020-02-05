
namespace EverythingNBA.Services
{
    using EverythingNBA.Services.Models.Team;

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPlayerService
    {
        Task<int> AddPlayerAsync(string firstName, string lastName, int teamId, int? rookieYear, int age, int height, int weight, string position,
            bool isStarter, IFormFile pictureFile, int shirtNumber, string instagramLink, string twitterLink);

        Task<bool> DeletePlayerAsync(int playerId);

       
    }
}
