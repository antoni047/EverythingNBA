namespace EverythingNBA.Web.Mapping
{
    using AutoMapper;
    using System.Linq;
    using System.Globalization;

    using Models.Seasons;
    using Models.AllStarTeams;
    using Models.Players;
    using Models.Games;
    using Services.Models.Season;
    using Services.Models.AllStarTeam;
    using Services.Models.Player;
    using Services.Models.Game;
    using Services.Models.GameStatistic;
    using EverythingNBA.Models;
    using Services.Models;
    using Services.Models.Playoff;
    using Services.Models.Team;
    using Services.Models.SeasonStatistic;
    using Services.Models.Series;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Web
            this.CreateMap<SeasonDetailsInputModel, GetSeasonListingServiceModel>();
            this.CreateMap<GetAllStarTeamServiceModel, EditAllStarTeamInputModel>();
            this.CreateMap<PlayerInputModel, PlayerDetailsServiceModel>();
            this.CreateMap<GameDetailsServiceModel, GameDetailsViewModel>();
            this.CreateMap<PlayerGameStatisticServiceModel, GameStatisticInputModel>();
            this.CreateMap<GameStatisticInputModel, PlayerGameStatisticServiceModel>().ReverseMap();



            //Services
            this.CreateMap<SeasonStatistic, GetSeasonStatisticDetailsServiceModel>()
               .ForMember(mdl => mdl.Id, opt => opt.MapFrom(ss => ss.Id))
               .ReverseMap();

            this.CreateMap<TeamSeasonStatisticServiceModel, SeasonStatisticOverviewServiceModel>()
                .ForMember(mdl => mdl.Position, opt => opt.Ignore());

            this.CreateMap<Season, GetSeasonListingServiceModel>()
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(s => s.Id));

            this.CreateMap<Playoff, GetPlayoffServiceModel>()
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(p => p.Id));

            this.CreateMap<Series, GetSeriesDetailsServiceModel>()
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(ss => ss.Id));
            this.CreateMap<Series, SeriesOverviewServiceModel>()
                .ForMember(mdl => mdl.Team1Name, opt => opt.MapFrom(s => s.Team1.Name))
                .ForMember(mdl => mdl.Team2Name, opt => opt.MapFrom(s => s.Team2.Name))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(ss => ss.Id));
            this.CreateMap<TopStatsServiceModel, GetSeriesDetailsServiceModel>();
            this.CreateMap<GetSeriesDetailsServiceModel, SeriesOverviewServiceModel>();

            this.CreateMap<Team, GetTeamDetailsServiceModel>()
                .ForMember(mdl => mdl.CurrentSeasonStatistic, opt => opt.Ignore())
                .ForMember(mdl => mdl.Last9Games, opt => opt.Ignore())
                .ForMember(mdl => mdl.Players, opt => opt.Ignore())
                .ForMember(mdl => mdl.TitlesWon, opt => opt.MapFrom(t => t.TitlesWon.Select(x => x.Year).ToList()))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(t => t.Id));

            this.CreateMap<Team, TeamOverviewServiceModel>()
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(t => t.Id));

            this.CreateMap<Player, PlayerOverviewServiceModel>()
                .ForMember(mdl => mdl.Name, opt => opt.MapFrom(p => p.FirstName + " " + p.LastName))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(p => p.Id));

            this.CreateMap<Player, TeamPlayerOverviewServiceModel>()
                 .ForMember(mdl => mdl.Name, opt => opt.MapFrom(p => p.FirstName + " " + p.LastName))
                 .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()))
                 .ForMember(mdl => mdl.Id, opt => opt.MapFrom(ss => ss.Id));

            this.CreateMap<Player, PlayerDetailsServiceModel>()
                .ForMember(mdl => mdl.CurrentTeam, opt => opt.MapFrom(p => p.Team.Name))
                .ForMember(mdl => mdl.Position, opt => opt.MapFrom(p => p.Position.ToString()))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(p => p.Id))
                .ReverseMap();

            this.CreateMap<Game, TeamGameOverviewServiceModel>()
                .ForMember(mdl => mdl.Date, opt => opt.MapFrom(g => g.Date.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(mdl => mdl.TeamHostName, opt => opt.MapFrom(g => g.TeamHost.Name))
                .ForMember(mdl => mdl.IsHomeGame, opt => opt.Ignore());

            this.CreateMap<Game, GameDetailsServiceModel>()
                .ForMember(mdl => mdl.Date, opt => opt.MapFrom(g => g.Date.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture)))
                .ForMember(mdl => mdl.TeamHostShortName, opt => opt.MapFrom(g => g.TeamHost.AbbreviatedName))
                .ForMember(mdl => mdl.Team2ShortName, opt => opt.MapFrom(g => g.Team2.AbbreviatedName))
                .ForMember(mdl => mdl.Venue, opt => opt.MapFrom(g => g.TeamHost.Venue))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(g => g.Id));


            this.CreateMap<GameStatistic, PlayerGameStatisticServiceModel>()
                .ForMember(mdl => mdl.PlayerName, opt => opt.MapFrom(gs => gs.Player.FirstName + " " + gs.Player.LastName))
                .ForMember(mdl => mdl.Id, opt => opt.MapFrom(gs => gs.Id));
        }
    }
}
