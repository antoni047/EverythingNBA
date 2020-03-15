namespace EverythingNBA.Services.Models.Playoff
{
    using Services.Models.Series;

    using System.Collections.Generic;

    public class GetPlayoffServiceModel
    {
        public int Id { get; set; }

        public int? SeasonId { get; set; }

        public string WinnerName { get; set; }

        public bool AreQuarterFinalsFinished { get; set; }

        public bool AreSemiFinalsFinished { get; set; }

        public bool AreConferenceFinalsFinished { get; set; }

        public List<SeriesOverviewServiceModel> Series { get; set; }
    }
}
