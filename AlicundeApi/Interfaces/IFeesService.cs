using AlicundeApi.Models;

namespace AlicundeApi.Interfaces
{
    public interface IFeesService
    {
        Task<List<Fees>> GetFeesAsync();
        Task FetchAndStoreNewFeesAsync();
    }
}
