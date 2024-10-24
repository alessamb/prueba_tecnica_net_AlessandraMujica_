using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AlicundeApi.DataBase;
using AlicundeApi.Interfaces;
using AlicundeApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

public class FeesService : IFeesService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _context; 
    private readonly ILogger<FeesService> _logger;

    public FeesService(HttpClient httpClient, AppDbContext context, ILogger<FeesService> logger)
    {
        _httpClient = httpClient;
        _context = context; 
        _logger = logger;
    }

    // Método para obtener las tarifas desde la API externa
    public async Task<List<Fees>> GetFeesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.opendata.esett.com/EXP05/Fees");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Fees>>(jsonResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error fetching fees: {ex.Message}");
            return new List<Fees>(); // Retornar una lista vacía en caso de error
        }
    }

    public async Task FetchAndStoreNewFeesAsync()
    {
        try
        {
            // Obtener tarifas desde la API
            var fees = await GetFeesAsync();

            // Verificar si hay tarifas para almacenar
            if (fees != null && fees.Count > 0)
            {
         
                // Obtener todas las tarifas existentes en la base de datos
                var existingFees = await _context.Fees.ToListAsync();
                var newFees = new List<Fees>();

                // Comparar cada tarifa obtenida de la API con las tarifas existentes
                foreach (var fee in fees)
                {
                    // Comparar por country y weeklyFee
                    if (!existingFees.Any(existingFee =>
                        existingFee.Country == fee.Country &&
                        existingFee.WeeklyFee == fee.WeeklyFee))
                    {
                        // Si la tarifa no existe, se agrega a la lista de nuevas tarifas
                        newFees.Add(fee);
                    }
                }

                // Si hay nuevas tarifas, se agregan a la base de datos
                if (newFees.Count > 0)
                {
                    _context.Fees.AddRange(newFees);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"{newFees.Count} new fees stored successfully.");
                }
                else
                {
                    _logger.LogWarning("All fetched fees already exist in the database. No new fees to store.");
                }
            }
            else
            {
                _logger.LogWarning("No fees found to store.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error storing fees: {ex.Message}");
            throw new ApplicationException("An error has occurred: " + ex.Message );
        }
    }

}
