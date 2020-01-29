namespace EverythingNBA.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Playoff
    {
        public int Id { get; set; }


        public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }

        public virtual Series WesternQuarterFinalFirst { get; set; }
        public virtual Series WesternQuarterFinalSecond { get; set; }
        public virtual Series WesternQuarterFinalThird { get; set; }
        public virtual Series WesternQuarterFinalFourth { get; set; }

        public virtual Series EasternQuarterFinalFirst { get; set; }
        public virtual Series EasternQuarterFinalSecond { get; set; }
        public virtual Series EasternQuarterFinalThird { get; set; }
        public virtual Series EasternQuarterFinalFourth { get; set; }

        public virtual Series WesternSemiFinalFirst { get; set; }
        public virtual Series WesternSemiFinalSecond { get; set; }

        public virtual Series EasternSemiFinalFirst { get; set; }
        public virtual Series EasternSemiFinalSecond { get; set; }

        public virtual Series WesternFinal { get; set; }

        public virtual Series EasternFinal { get; set; }

        public virtual Series Final { get; set; }

    }
}
