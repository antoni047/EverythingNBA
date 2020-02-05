namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.Team;
    using EverythingNBA.Services.Models.SeasonStatistic;

    using Microsoft.AspNetCore.Http;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using EverythingNBA.Services.Models.Player;

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
            var standingsList = new List<TeamStandingsListingServiceModel>();

            var teams = this.db.Teams.ToList();

            foreach (var team in teams)
            {
                var model = await statisticService.GetBySeasonAndTeamAsync(seasonId, team.Id);
                var seasonStatistic = Mapper.Map<SeasonStatistic>(model);
                var winPercentage = await statisticService.GetWinPercentageAsync(seasonStatistic.Id);

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

            return standingsList.OrderByDescending(x => x.WinPercentage).ToList();
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var seasonId = await this.db.Seasons.Where(s => s.Year == DateTime.Now.Year).Select(s => s.Id).FirstOrDefaultAsync();

            var model = Mapper.Map<GetTeamDetailsServiceModel>(team);

            var players = team.Players.ToList();
            var playerModels = new List<PlayerOverviewServiceModel>();

            foreach (var player in players)
            {
                var playerOverviewModel = Mapper.Map<PlayerOverviewServiceModel>(player);

                var pointsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Points).FirstOrDefault();
                var assistsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Assists).FirstOrDefault();
                var reboundsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.AverageRebounds).FirstOrDefault();

                playerOverviewModel.PointsPerGame = pointsPerGame;
                playerOverviewModel.AssistsPerGame = assistsPerGame;
                playerOverviewModel.ReboundsPerGame = reboundsPerGame;

                playerModels.Add(playerOverviewModel);
            }

            model.CurrentPlayers = playerModels;

            var games = new List<Game>();
            games.AddRange(team.HomeGames);
            games.AddRange(team.AwayGames);
            model.CurrentSeasonGames = games.OrderByDescending(g => g.Date).ToList();


            var teamSeasonStatistic = team.SeasonsStatistics.Where(ss => ss.SeasonId == seasonId).FirstOrDefault();
            model.CurrentSeasonStatistic = Mapper.Map<GetSeasonStatisticDetailsServiceModel>(teamSeasonStatistic);

            model.TitlesWon = team.TitlesWon.Select(t => t.Year).ToList();

            return model;
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name)
        {
            var teamId = await this.db.Teams.Where(t => t.Name.ToLower() == name.ToLower()).Select(t => t.Id).FirstOrDefaultAsync();

            return await this.GetTeamDetailsAsync(teamId);
        }
    }
}
