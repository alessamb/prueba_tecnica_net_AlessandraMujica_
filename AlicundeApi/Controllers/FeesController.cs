using AlicundeApi.DataBase;
using AlicundeApi.Interfaces;
using AlicundeApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlicundeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeesController : ControllerBase
    {
        private readonly IFeesService _feesService;
        private readonly AppDbContext _context;

        public FeesController(IFeesService feesService, AppDbContext context)
        {
            _feesService = feesService;
            _context = context;
        }

        /// <summary>
        /// Recupera y almacena nuevas tarifas desde una fuente externa, y devuelve la lista actualizada de tarifas en la base de datos.
        /// </summary>
        /// <returns>Una acción que contiene la lista actualizada de tarifas.</returns>

        [HttpGet()]
        public async Task<ActionResult<Fees>> FetchAndStoreNewFees()
        {
            await _feesService.FetchAndStoreNewFeesAsync();
            var fees = await _context.Fees.ToListAsync();
            return Ok(fees);
        }
       
        
        /// <summary>
        /// Obtiene una tarifa específica por su ID.
        /// </summary>
        /// <param name="id">El identificador de la tarifa a recuperar.</param>
        /// <returns>El objeto Fees correspondiente al ID proporcionado, o un resultado NotFound si no se encuentra.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Fees>>> GetFeesForId(int id)
        {
            var fees = await _context.Fees.FindAsync(id);
        
              if (fees == null )
              {
                return NotFound("No fees found.");
            }
            return Ok(fees);

        }
 

    }
}
