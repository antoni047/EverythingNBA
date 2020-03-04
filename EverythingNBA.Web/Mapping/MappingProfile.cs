namespace EverythingNBA.Web.Mapping
{
    using AutoMapper;
    using Models.Seasons;
    using Models.AllStarTeams;
    using Services.Models.Season;
    using Services.Models.AllStarTeam;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonDetailsInputModel, GetSeasonListingServiceModel>();
            this.CreateMap<GetAllStarTeamServiceModel, EditAllStarTeamInputModel>();
        }
    }
}
