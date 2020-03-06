namespace EverythingNBA.Models
{
    using System.Collections.Generic;

    public class Playoff
    {
        public int Id { get; set; }


        public int? SeasonId { get; set; }
        public virtual Season Season { get; set; }

        public ICollection<Series> Series { get; set; }

        //public int? WesternQuarterFinalFirstId { get; set; }
        //public virtual Series WesternQuarterFinalFirst { get; set; }
        //public int? WesternQuarterFinalSecondId { get; set; }
        //public virtual Series WesternQuarterFinalSecond { get; set; }
        //public int? WesternQuarterFinalThirdId { get; set; }
        //public virtual Series WesternQuarterFinalThird { get; set; }
        //public int? WesternQuarterFinalFourthId { get; set; }
        //public virtual Series WesternQuarterFinalFourth { get; set; }


        //public int? EasternQuarterFinalFirstId { get; set; }
        //public virtual Series EasternQuarterFinalFirst { get; set; }
        //public int? EasternQuarterFinalSecondId { get; set; }
        //public virtual Series EasternQuarterFinalSecond { get; set; }
        //public int? EasternQuarterFinalThirdId { get; set; }
        //public virtual Series EasternQuarterFinalThird { get; set; }
        //public int? EasternQuarterFinalFourthId { get; set; }
        //public virtual Series EasternQuarterFinalFourth { get; set; }

        //public int? WesternSemiFinalFirstId { get; set; }
        //public virtual Series WesternSemiFinalFirst { get; set; }
        //public int? WesternSemiFinalSecondId { get; set; }
        //public virtual Series WesternSemiFinalSecond { get; set; }

        //public int? EasternSemiFinalFirstId { get; set; }
        //public virtual Series EasternSemiFinalFirst { get; set; }
        //public int? EasternSemiFinalSecondId { get; set; }
        //public virtual Series EasternSemiFinalSecond { get; set; }

        //public int? WesternFinalId { get; set; }
        //public virtual Series WesternFinal { get; set; }

        //public int? EasternFinalId { get; set; }
        //public virtual Series EasternFinal { get; set; }

        //public int? FinalId { get; set; }
        //public virtual Series Final { get; set; }

    }
}
