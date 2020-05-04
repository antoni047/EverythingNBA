namespace EverythingNBA.Services.Implementations
{
    using System.Threading.Tasks;
    using System.Linq;
    using System.Collections.Generic;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;

    using EverythingNBA.Models;
    using Data;
    using Services.Models.Series;
    using Services.Models.Game;

    public class SeriesService : ISeriesService
    {
        private readonly EverythingNBADbContext db;
        private readonly IMapper mapper;

        public SeriesService(EverythingNBADbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task AddGameAsync(int seriesId, int gameId, int gameNumber)
        {
            var series = await this.db.Series.FindAsync(seriesId);

            switch (gameNumber)
            {
                case 1:
                    series.Game1Id = gameId;
                    break;
                case 2:
                    series.Game2Id = gameId;
                    break;
                case 3:
                    series.Game3Id = gameId;
                    break;
                case 4:
                    series.Game4Id = gameId;
                    break;
                case 5:
                    series.Game5Id = gameId;
                    break;
                case 6:
                    series.Game6Id = gameId;
                    break;
                case 7:
                    series.Game7Id = gameId;
                    break;
            } //sets gameId by the given game number

            await this.db.SaveChangesAsync();
        }

        public async Task<int> AddSeriesAsync(int playoffId, string team1Name, string team2Name, int team1GameWon, int team2GamesWon, int? game1Id, int? game2Id,
            int? game3Id, int? game4Id, int? game5Id, int? game6Id, int? game7Id, string conference, string stage, int stageNumber,
            int? team1Position, int? team2Position)
        {
            var team1Id = await this.db.Teams.Where(t => t.Name == team1Name).Select(t => t.Id).FirstOrDefaultAsync();
            var team2Id = await this.db.Teams.Where(t => t.Name == team2Name).Select(t => t.Id).FirstOrDefaultAsync();

            var seriesObj = new Series
            {
                PlayoffId = playoffId,
                Team1Id = team1Id,
                Team2Id = team2Id,
                Team1GamesWon = team1GameWon,
                Team2GamesWon = team2GamesWon,
                Game1Id = game1Id,
                Game2Id = game2Id,
                Game3Id = game3Id,
                Game4Id = game4Id,
                Game5Id = game5Id,
                Game6Id = game6Id,
                Game7Id = game7Id,
                Conference = conference,
                Stage = stage,
                StageNumber = stageNumber,
                Team1StandingsPosition = team1Position != null ? (int)team1Position : 0,
                Team2StandingsPosition = team2Position != null ? (int)team2Position : 0,
            };

            if (stage == "QuarterFinal")
            {
                switch (stageNumber)
                {
                    case 1:
                        seriesObj.Team1StandingsPosition = 1;
                        seriesObj.Team2StandingsPosition = 8;
                        break;

                    case 2:
                        seriesObj.Team1StandingsPosition = 4;
                        seriesObj.Team2StandingsPosition = 5;
                        break;

                    case 3:
                        seriesObj.Team1StandingsPosition = 3;
                        seriesObj.Team2StandingsPosition = 6;
                        break;

                    case 4:
                        seriesObj.Team1StandingsPosition = 2;
                        seriesObj.Team2StandingsPosition = 7;
                        break;
                }
            }

            this.db.Series.Add(seriesObj);
            await this.db.SaveChangesAsync();

            return seriesObj.Id;
        }

        public async Task<bool> DeleteSeriesAsync(int seriesId)
        {
            var seriesToDelete = await this.db.Series.FindAsync(seriesId);

            if (seriesToDelete == null)
            {
                return false;
            }

            this.db.Series.Remove(seriesToDelete);
            await this.db.SaveChangesAsync();

            return true;
        }

        public async Task<GetSeriesDetailsServiceModel> GetSeriesAsync(int id)
        {
            var series = await this.db.Series
                .Include(s => s.Team1)
                .Include(s => s.Team2)
                .Include(s => s.Game1).Include(s => s.Game2).Include(s => s.Game3).Include(s => s.Game4)
                .Include(s => s.Game5).Include(s => s.Game6).Include(s => s.Game6)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            var model = mapper.Map<GetSeriesDetailsServiceModel>(series);

            var game1 = mapper.Map<GameDetailsServiceModel>(series.Game1) ?? null;
            var game2 = mapper.Map<GameDetailsServiceModel>(series.Game2) ?? null;
            var game3 = mapper.Map<GameDetailsServiceModel>(series.Game3) ?? null;
            var game4 = mapper.Map<GameDetailsServiceModel>(series.Game4) ?? null;
            var game5 = mapper.Map<GameDetailsServiceModel>(series.Game5) ?? null;
            var game6 = mapper.Map<GameDetailsServiceModel>(series.Game6) ?? null;
            var game7 = mapper.Map<GameDetailsServiceModel>(series.Game7) ?? null;

            model.Games = new List<GameDetailsServiceModel>() { game1, game2, game3, game4, game5, game6, game7 };

            return model;
        }

        public async Task<SeriesOverviewServiceModel> GetSeriesOverview(int seriesId)
        {
            var series = await this.GetSeriesAsync(seriesId);

            var model = mapper.Map<SeriesOverviewServiceModel>(series);


            return model;
        }

        public async Task<SeriesWinnerServiceModel> GetWinnerAsync(int seriesId)
        {
            var series = await this.db.Series
                .Include(s => s.Team1)
                .Include(s => s.Team2)
                .Where(s => s.Id == seriesId)
                .FirstOrDefaultAsync();

            var winner = mapper.Map<SeriesWinnerServiceModel>(series);

            if (series.Team1GamesWon > series.Team2GamesWon)
            {
                winner.TeamName = series.Team1.Name;
                winner.StandingsPosition = series.Team1StandingsPosition;
            }
            else
            {
                winner.TeamName = series.Team2.Name;
                winner.StandingsPosition = series.Team2StandingsPosition;
            }

            return winner;
        }

        public async Task SetGameWon(int seriesId, string winner)
        {
            var series = await this.db.Series
                .Include(s => s.Team1)
                .Include(s => s.Team2)
                .Where(s => s.Id == seriesId).FirstOrDefaultAsync();

            if (series.Team1GamesWon != 4 && series.Team2GamesWon != 4)
            {
                if (series.Team1.Name == winner)
                {
                    series.Team1GamesWon++;
                }
                else if (series.Team2.Name == winner)
                {
                    series.Team2GamesWon++;
                }
            }

            await this.db.SaveChangesAsync();
        }
    }
}
