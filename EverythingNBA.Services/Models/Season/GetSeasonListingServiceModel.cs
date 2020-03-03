namespace EverythingNBA.Services.Models.Season
{
    public class GetSeasonListingServiceModel
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string TitleWinnerName { get; set; }

        public int PlayoffId { get; set; }
    }
}
