using AlicundeApi.Models;

namespace AlicundeApi.Interfaces
{
    public interface IBankService
    {
        Task<List<Bank>> GetBanksAsync();
        Task FetchAndStoreNewBanksAsync();
    }
}
