namespace EverythingNBA.Services.Implementations
{
    using EverythingNBA.Data;
    using EverythingNBA.Models;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.Team;

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
        private readonly IMapper mapper;

        public TeamService(EverythingNBADbContext db, IImageService imageService, ISeasonStatisticService statisticService, IMapper mapper)
        {
            this.db = db;
            this.imageService = imageService;
            this.statisticService = statisticService;
            this.mapper = mapper;
        }

        public async Task AddPlayerAsync(int playerId, int teamId)
        {
            var player = await this.db.FindAsync<Player>(playerId);
            var team = await this.db.FindAsync<Team>(teamId);

            var someting = string.Empty;

            team.Players.Add(player);
            player.TeamId = teamId;
            player.Team = team;

            await this.db.SaveChangesAsync();
        }

        public async Task<bool> RemovePlayerAsync(int playedId, int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var player = team.Players.Where(p => p.Id == playedId).FirstOrDefault();

            if (player == null)
            {
                return false;
            }

            team.Players.Remove(player);
            player.TeamId = null;
            player.Team = null;

            await this.db.SaveChangesAsync();

            return true;

        }

        public async Task<int> AddTeamAsync(string name, /*IFormFile imageFile,*/ string conference, string venue, string instagram, string twitter)
        {
            //var imageId = await this.imageService.UploadImageAsync(imageFile);

            var teamObj = new Team
            {
                Name = name,
                //CloudinaryImageId = imageId,
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
                var model = await statisticService.GetDetailsAsync(seasonId, team.Id);
                var seasonStatistic = mapper.Map<SeasonStatistic>(model);
                var winPercentage = await statisticService.GetWinPercentageAsync(seasonStatistic.Id);

                var result = new TeamStandingsListingServiceModel
                {
                    Name = team.Name,
                    //TeamLogoImageURL = team.CloudinaryImage.ImageURL,
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
            var team = await this.db.Teams.Where(t => t.Id == teamId).FirstOrDefaultAsync();
            var seasonId = await this.db.Seasons.Where(s => s.Year == DateTime.Now.Year).Select(s => s.Id).FirstOrDefaultAsync();

            var model = mapper.Map<GetTeamDetailsServiceModel>(team);

            var players = team.Players.ToList();
            var playerModels = new List<PlayerOverviewServiceModel>();

            foreach (var player in players)
            {
                var playerOverviewModel = mapper.Map<PlayerOverviewServiceModel>(player);

                var pointsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Points).FirstOrDefault();
                var assistsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.Assists).FirstOrDefault();
                var reboundsPerGame = player.SeasonStatistics.Where(ss => ss.SeasonId == seasonId).Select(ss => ss.AverageRebounds).FirstOrDefault();

                playerOverviewModel.PointsPerGame = pointsPerGame;
                playerOverviewModel.AssistsPerGame = assistsPerGame;
                playerOverviewModel.ReboundsPerGame = reboundsPerGame;

                playerModels.Add(playerOverviewModel);
            }

            model.Players = playerModels;

            var games = new List<Game>();
            games.AddRange(team.HomeGames);
            games.AddRange(team.AwayGames);
            model.CurrentSeasonGames = games.OrderByDescending(g => g.Date).ToList();
         
            model.CurrentSeasonStatistic = team.SeasonsStatistics.Where(ss => ss.SeasonId == seasonId && ss.TeamId == teamId).FirstOrDefault();

            model.TitlesWon = team.TitlesWon.Select(t => t.Year).ToList();

            return model;
        }

        public async Task<GetTeamDetailsServiceModel> GetTeamDetailsAsync(string name)
        {
            var teamId = await this.db.Teams.Where(t => t.Name.ToLower() == name.ToLower()).Select(t => t.Id).FirstOrDefaultAsync();

            return await this.GetTeamDetailsAsync(teamId);
        }

        public async Task<bool> AddGameAsync(int gameId, int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var game = await this.db.Games.FindAsync(gameId);
            bool teamIsHost;

            if (game.TeamHostId == teamId)
            {
                team.HomeGames.Add(game);
                teamIsHost = true;
            }
            else if(game.Team2Id == teamId)
            {
                team.AwayGames.Add(game);
                teamIsHost = false;
            }
            else
            {
                return false;
            }

            if (game.IsFinished)
            {
                if (game.TeamHostPoints > game.Team2Points && teamIsHost)
                {
                    team.GamesWon.Add(game);
                }
                else
                {
                    if (teamIsHost == false)
                    {
                        team.GamesWon.Add(game);
                    }
                }
            }

            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveGameAsync(int gameId, int teamId)
        {
            var team = await this.db.Teams.FindAsync(teamId);

            var game = await this.db.Games.FindAsync(gameId);

            if (game == null || team == null)
            {
                return false;
            }

            if (team.HomeGames.Contains(game))
            {
                team.HomeGames.Remove(game);

                await this.db.SaveChangesAsync();
                return true;
            }

            else if (team.AwayGames.Contains(game))
            {
                team.AwayGames.Remove(game);

                await this.db.SaveChangesAsync();
                return true;
            }

            else
            {
                return false;
            }
        }

        public async Task<bool> AddTitleAsync(int teamId, int seasonId)
        {
            var team = await this.db.FindAsync<Team>(teamId);
            var season = await this.db.FindAsync<Season>(seasonId);

            if (team == null || season == null)
            {
                return false;
            }

            team.TitlesWon.Add(season);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTitleAsync(int teamId, int seasonId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var season = await this.db.Seasons.FindAsync(seasonId);

            if (team == null || season == null)
            {
                return false;
            }

            team.TitlesWon.Remove(season);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddSeasonStatistic(int teamId, int seasonStatisticId)
        {
            var team = await this.db.FindAsync<Team>(teamId);
            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            var someting = string.Empty;

            if (team == null || seasonStatistic == null)
            {
                return false;
            }

            team.SeasonsStatistics.Add(seasonStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveSeasonStatistic(int teamId, int seasonStatisticId)
        {
            var team = await this.db.Teams.FindAsync(teamId);
            var seasonStatistic = await this.db.SeasonStatistics.FindAsync(seasonStatisticId);

            var someting = string.Empty;

            if (team == null || seasonStatistic == null)
            {
                return false;
            }

            team.SeasonsStatistics.Remove(seasonStatistic);

            await this.db.SaveChangesAsync();
            return true;
        }
    }
}
