namespace EverythingNBA.Services.Models.Game
{
    public class TeamGameOverviewServiceModel
    {
        public int Id { get; set; }

        public string TeamHostName { get; set; }

        public string Team2Name { get; set; }

        public bool IsHomeGame { get; set; }

        public string Date { get; set; }

        public int TeamHostPoints { get; set; }

        public int Team2Points { get; set; }
    }
}
