namespace EverythingNBA.Web.Models.Players
{
    using System;
    using System.Collections.Generic;

    public class AllPlayersViewModel
    {
        public ICollection<string> PlayerNames { get; set; }

        public int Total { get; set; }

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        public bool PreviousPageIsDisabled => this.CurrentPage == 1;

        public bool NextPageIsDisabled 
        {
            get 
            {
                var maxPage = Math.Ceiling(((double)this.Total) / 25);

                return maxPage == this.CurrentPage;
            }
        }
    }
}
