namespace EverythingNBA.Services.Models.Series
{
    public class SeriesWinnerServiceModel
    {
        public int Id { get; set; }

        public int PlayoffId { get; set; }

        public string TeamName { get; set; }

        public int StandingsPosition { get; set; }
    }
}
