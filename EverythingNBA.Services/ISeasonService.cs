namespace EverythingNBA.Services
{
    using System.Threading.Tasks;

    public interface ISeasonService
    {
        Task<int> AddAsync(int year, int? titleWinnerId, int? playoffId);

        Task<bool> DeleteAsync (int seasonId);
    }
}
