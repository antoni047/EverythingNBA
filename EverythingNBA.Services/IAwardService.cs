namespace EverythingNBA.Services
{
    using System;
    using System.Threading.Tasks;

    public interface IAwardService
    {
        Task<int> AddAwardAsync(string name, int year, int winnerId);

        Task<bool> DeleteAwardAsync(int awardId);
    }
}
