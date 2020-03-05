namespace EverythingNBA.Services.Models.Playoff
{
    using Services.Models.Series;
    public class GetPlayoffServiceModel
    {
        public int Id { get; set; }

        public int? SeasonId { get; set; }


        public SeriesOverviewServiceModel WesternQuarterFinalFirst { get; set; }

        public SeriesOverviewServiceModel WesternQuarterFinalSecond { get; set; }

        public SeriesOverviewServiceModel WesternQuarterFinalThird { get; set; }

        public SeriesOverviewServiceModel WesternQuarterFinalFourth { get; set; }

        public SeriesOverviewServiceModel EasternQuarterFinalFirst { get; set; }

        public SeriesOverviewServiceModel EasternQuarterFinalSecond { get; set; }

        public SeriesOverviewServiceModel EasternQuarterFinalThird { get; set; }

        public SeriesOverviewServiceModel EasternQuarterFinalFourth { get; set; }

        public SeriesOverviewServiceModel WesternSemiFinalFirst { get; set; }

        public SeriesOverviewServiceModel WesternSemiFinalSecond { get; set; }

        public SeriesOverviewServiceModel EasternSemiFinalFirst { get; set; }

        public SeriesOverviewServiceModel EasternSemiFinalSecond { get; set; }

        public SeriesOverviewServiceModel WesternFinal { get; set; }

        public SeriesOverviewServiceModel EasternFinal { get; set; }

        public SeriesOverviewServiceModel Final { get; set; }

        public string WinnerName { get; set; }
    }
}
