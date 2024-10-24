using AlicundeApi.DataBase;
using AlicundeApi.Interfaces;
using AlicundeApi.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;

namespace AlicundeApi.Services
{
    public class BankService : IBankService
    {
        private readonly HttpClient _httpClient;
        private readonly AppDbContext _context;
        private readonly ILogger<BankService> _logger;

        public BankService(HttpClient httpClient, AppDbContext context, ILogger<BankService> logger)
        {
            _httpClient = httpClient;
            _context = context;
            _logger = logger;
        }

        public async Task<List<Bank>> GetBanksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("https://api.opendata.esett.com/EXP06/Banks");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var bankInfo = JsonConvert.DeserializeObject<List<Bank>>(jsonResponse);

                if (bankInfo == null)
                {
                    throw new Exception("No se pudieron deserializar los datos de la API.");
                }

                return bankInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching banks: {ex.Message}");
                return new List<Bank>();
            }
        }

        public async Task FetchAndStoreNewBanksAsync()
        {

            var banks = await GetBanksAsync();

            try
            {
                if (banks != null && banks.Count > 0)
                {
                    // Obtener los BICs de los bancos que vamos a agregar
                    var bicCodes = banks.Select(b => b.Bic).ToList();

                    // Buscar bancos existentes en la base de datos que coincidan con los BICs
                    var existingBanksBics = await _context.Banks
                        .Where(b => bicCodes.Contains(b.Bic))
                        .Select(b => b.Bic)
                        .ToListAsync();

                    // Filtrar los bancos que ya existen para no duplicarlos
                    var newBanks = banks
                        .Where(b => !existingBanksBics.Contains(b.Bic))
                        .ToList();

                    if (newBanks.Count > 0)
                    {
                        _context.Banks.AddRange(newBanks);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation($"{newBanks.Count} banks stored successfully.");
                    }
                    else
                    {
                        _logger.LogWarning("All banks already exist in the database. No new banks to store.");
                    }
                }
                else
                {
                    _logger.LogWarning("No banks found to store.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching banks: {ex.Message}");
            }
        }

    }
}
