namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models;

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Collections.Generic;

    public class TeamService : ITeamService
    {
        private readonly EverythingNBADbContext db;
        private readonly IImageService imageService;
        private readonly ISeasonStatisticService statisticService;

        public TeamService(EverythingNBADbContext db, IImageService imageService, ISeasonStatisticService statisticService)
        {
            this.db = db;
            this.imageService = imageService;
            this.statisticService = statisticService;
        }

        public async Task<int> AddTeamAsync(string name, IFormFile imageFile, string conference, string venue, string instagram, string twitter)
        {
            var imageId = await this.imageService.UploadImageAsync(imageFile);

            var teamObj = new Team
            {
                Name = name,
                CloudinaryImageId = imageId,
                Conference = (ConferenceType)Enum.Parse(typeof(ConferenceType), conference),
                Venue = venue,
                Twitter = twitter,
                Instagram = instagram
            };

            this.db.Teams.Add(teamObj);
            await this.db.SaveChangesAsync();

            return teamObj.Id;
        }

        public async Task<bool> DeleteTeamAsync(int teamId)
        {
            var teamToDelete = await this.db.Teams.FindAsync(teamId);

            if (teamToDelete == null)
            {
                return false;
            }

            this.db.Teams.Remove(teamToDelete);

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<ICollection<TeamStandingsListingServiceModel>> GetStandingsAsync(int seasonId)
        {
            var teams = this.db.Teams.ToList();

            var standingsList = new List<TeamStandingsListingServiceModel>();

            foreach (var team in teams)
            {
                var model = statisticService.GetAsync(seasonId, team.Id);
                var seasonStatistic = Mapper.Map<SeasonStatistic>(model);
                var winPercentage = statisticService.GetWinPercentageAsync(seasonStatistic.Id);

                var result = new TeamStandingsListingServiceModel
                {
                    Name = team.Name,
                    TeamLogoImageURL = team.CloudinaryImage.ImageURL,
                    Conference = team.Conference.ToString(),
                    Wins = seasonStatistic.Wins,
                    Losses = seasonStatistic.Losses,
                    WinPercentage = winPercentage.ToString(),
                };

                standingsList.Add(result);
            }

            return standingsList;
        }
    }
}
