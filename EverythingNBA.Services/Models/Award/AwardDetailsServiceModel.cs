namespace EverythingNBA.Services.Models.Award
{
    public class AwardDetailsServiceModel
    {
        public int Id { get; set; }

        public string Winner { get; set; }

        public string Type { get; set; }

        public string WinnerTeam { get; set; }

        public int Year { get; set; }
    }
}
