namespace EverythingNBA.Web.Models.Awards
{
    public class AddAwardInputModel
    {
        public string Type { get; set; }

        public string WinnerName { get; set; }

        public string WinnerTeamName { get; set; }

        public int Year { get; set; }
    }
}
