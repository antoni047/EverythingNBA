using System;
using System.Collections.Generic;
using System.Text;

namespace EverythingNBA.Services.Models.Playoff
{
    public class GetPlayoffServiceModel
    {
        public int Id { get; set; }

        public int? SeasonId { get; set; }


        public int? WesternQuarterFinalFirstId { get; set; }

        public int? WesternQuarterFinalSecondId { get; set; }

        public int? WesternQuarterFinalFourthId { get; set; }

        public int? EasternQuarterFinalFirstId { get; set; }

        public int? EasternQuarterFinalSecondId { get; set; }

        public int? EasternQuarterFinalThirdId { get; set; }

        public int? EasternQuarterFinalFourthId { get; set; }

        public int? WesternSemiFinalFirstId { get; set; }

        public int? WesternSemiFinalSecondId { get; set; }

        public int? EasternSemiFinalFirstId { get; set; }

        public int? EasternSemiFinalSecondId { get; set; }

        public int? WesternFinalId { get; set; }

        public int? EasternFinalId { get; set; }

        public int? FinalId { get; set; }

    }
}
