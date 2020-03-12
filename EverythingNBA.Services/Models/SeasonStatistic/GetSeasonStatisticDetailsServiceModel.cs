namespace EverythingNBA.Services.Models.SeasonStatistic
{
    public class GetSeasonStatisticDetailsServiceModel
    {
        public int Id { get; set; }

        public int TeamId { get; set; }

        public int? SeasonId { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }
    }
}
