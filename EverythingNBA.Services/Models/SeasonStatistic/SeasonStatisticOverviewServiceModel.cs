namespace EverythingNBA.Services.Models.SeasonStatistic
{
    public class SeasonStatisticOverviewServiceModel
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public string Name { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public int GamesPlayed { get; set; }

        public string LastTenGames { get; set; }
    }
}
