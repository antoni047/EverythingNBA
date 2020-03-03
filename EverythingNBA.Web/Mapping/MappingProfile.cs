namespace EverythingNBA.Web.Mapping
{
    using AutoMapper;
    using Models.Seasons;
    using Services.Models.Season;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<SeasonDetailsInputModel, GetSeasonListingServiceModel>();
        }
    }
}
