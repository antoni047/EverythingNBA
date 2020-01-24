namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Playoff
    {
        public int Id { get; set; }

       
        public int? SeasonId { get; set; }
        public Season Season { get; set; }

        public Series WesternQuarterFinalFirst { get; set; }
        public Series WesternQuarterFinalSecond { get; set; }
        public Series WesternQuarterFinalThird { get; set; }
        public Series WesternQuarterFinalFourth { get; set; }

        public Series EasternQuarterFinalFirst { get; set; }
        public Series EasternQuarterFinalSecond { get; set; }
        public Series EasternQuarterFinalThird { get; set; }
        public Series EasternQuarterFinalFourth { get; set; }

        public Series WesternSemiFinalFirst { get; set; }
        public Series WesternSemiFinalSecond { get; set; }

        public Series EasternSemiFinalFirst { get; set; }
        public Series EasternSemiFinalSecond { get; set; }

        public Series WesternFinal  { get; set; }

        public Series EasternFinal  { get; set; }

        public Series Final { get; set; }

    }
}
