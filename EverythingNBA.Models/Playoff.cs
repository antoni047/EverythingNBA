namespace EverythingNBA.Models
{
    using System.Collections.Generic;

    public class Playoff
    {
        public int Id { get; set; }

        public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }

        public bool AreQuarterFinalsFinished { get; set; }

        public bool AreSemiFinalsFinished { get; set; }

        public bool AreConferenceFinalsFinished { get; set; }

        public ICollection<Series> Series { get; set; }
    }
}
