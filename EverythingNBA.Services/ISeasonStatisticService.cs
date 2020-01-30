namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    public interface ISeasonStatisticService
    {
        Task<int> AddAsync(int seasonId, int wins, int losses);

        Task<bool> DeleteAsync(int seasonStatisticId);
    }
}
