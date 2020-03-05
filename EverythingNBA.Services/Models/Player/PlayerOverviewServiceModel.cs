namespace EverythingNBA.Services.Models.Player
{
    public class PlayerOverviewServiceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public double PointsPerGame { get; set; }

        public double AssistsPerGame { get; set; }

        public double ReboundsPerGame { get; set; }

        public string Position { get; set; }
    }
}
