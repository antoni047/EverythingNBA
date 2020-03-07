namespace EverythingNBA.Services.Mapping
{
    using AutoMapper;
    using System.Linq;
    using System;

    using EverythingNBA.Models;
    using EverythingNBA.Services.Models;
    using EverythingNBA.Services.Models.AllStarTeam;
    using EverythingNBA.Services.Models.Season;
    using EverythingNBA.Services.Models.Playoff;
    using EverythingNBA.Services.Models.Player;
    using EverythingNBA.Services.Models.Team;
    using EverythingNBA.Services.Models.Game;
    using Services.Models.SeasonStatistic;
    using EverythingNBA.Services.Models.GameStatistic;
    using EverythingNBA.Models.Enums;
    using EverythingNBA.Services.Models.Series;
    using System.Globalization;
    using Services.Models.GameStatisticModels;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonStatistic, GetSeasonStatisticDetailsServiceModel>().ReverseMap();

            this.CreateMap<TeamSeasonStatisticServiceModel, SeasonStatisticOverviewServiceModel>()
                .ForMember(mdl => mdl.Position, opt => opt.Ignore());

            this.CreateMap<Season, GetSeasonListingServiceModel>();

            this.CreateMap<Playoff, GetPlayoffServiceModel>();

            this.CreateMap<Series, GetSeriesDetailsServiceModel>();
            this.CreateMap<Series, SeriesOverviewServiceModel>()
                .ForMember(mdl => mdl.Team1Name, opt => opt.Ignore())
                .ForMember(mdl => mdl.Team2Name, opt => opt.Ignore());
            this.CreateMap<TopStatsServiceModel, GetSeriesDetailsServiceModel>();
            this.CreateMap<GetSeriesDetailsServiceModel, SeriesOverviewServiceModel>();

            this.CreateMap<Team, GetTeamDetailsServiceModel>()
                .ForMember(mdl => mdl.CurrentSeasonStatistic, opt => opt.Ignore())
                .ForMember(mdl => mdl.Last9Games, opt => opt.Ignore())
                .ForMember(mdl => mdl.Players, opt => opt.Ignore())
                .ForMember(mdl => mdl.TitlesWon, opt => opt.MapFrom(t => t.TitlesWon.Select(x => x.Year).ToList()));

            this.CreateMap<Player, PlayerOverviewServiceModel>()
                .ForMember(mdl => mdl.Name, opt => opt.MapFrom(p => p.FirstName + " " + p.LastName))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()));

           this.CreateMap<Player, TeamPlayerOverviewServiceModel>()
                .ForMember(mdl => mdl.Name, opt => opt.MapFrom(p => p.FirstName + " " + p.LastName))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()));

            this.CreateMap<Player, PlayerDetailsServiceModel>()
                .ForMember(mdl => mdl.CurrentTeam, opt => opt.MapFrom(p => p.Team.Name))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()))
                .ReverseMap();

            this.CreateMap<Game, GameOverviewServiceModel>()
                .ForMember(mdl => mdl.Date, opt => opt.MapFrom(g => g.Date.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture)));

            this.CreateMap<Game, TeamGameOverviewServiceModel>()
                .ForMember(mdl => mdl.Date, opt => opt.MapFrom(g => g.Date.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(mdl => mdl.TeamHostName, opt => opt.MapFrom(g => g.TeamHost.Name))
                .ForMember(mdl => mdl.IsHomeGame, opt => opt.Ignore());

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(mdl => mdl.PlayerStats, opt => opt.Ignore());

            this.CreateMap<GameStatistic, PlayerGameStatisticServiceModel>()
                .ForMember(mdl => mdl.PlayerName, opt => opt.MapFrom(gs => gs.Player.FirstName + " " + gs.Player.LastName));
        }
    }
}
