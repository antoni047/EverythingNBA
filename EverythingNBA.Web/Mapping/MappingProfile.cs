﻿namespace EverythingNBA.Web.Mapping
{
    using AutoMapper;
    using Models.Seasons;
    using Models.AllStarTeams;
    using Models.Players;
    using Models.Games;
    using Services.Models.Season;
    using Services.Models.AllStarTeam;
    using Services.Models.Player;
    using Services.Models.Game;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonDetailsInputModel, GetSeasonListingServiceModel>();
            this.CreateMap<GetAllStarTeamServiceModel, EditAllStarTeamInputModel>();
            this.CreateMap<PlayerInputModel, PlayerDetailsServiceModel>();
            this.CreateMap<GameDetailsServiceModel, GameDetailsViewModel>();
        }
    }
}
