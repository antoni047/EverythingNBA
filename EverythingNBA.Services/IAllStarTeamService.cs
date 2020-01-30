namespace EverythingNBA.Services
{
    using System.Threading.Tasks;
    using System;

    public interface IAllStarTeamService
    {
        Task<int> AddAllStarTeamAsync(int year, string type);

        Task<bool> DeleteAllStarTeamAsync(int allStarTeamId);
    }
}
