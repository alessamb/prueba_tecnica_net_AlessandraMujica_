using AlicundeApi.DataBase;
using AlicundeApi.Interfaces;
using AlicundeApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlicundeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {
        private readonly IBankService _bankService;
        private readonly AppDbContext _context;


        public BankController(IBankService bankService, AppDbContext context)
        {
            _bankService = bankService;
            _context = context;
           
        }

        /// <summary>
        /// Obtiene una lista de todos los bancos en la base de datos.
        /// </summary>
        /// <returns>Una lista de objetos Bank.</returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bank>>> GetBanks()
        {
            
            var banks = await _context.Banks.ToListAsync();

            if (banks == null || banks.Count == 0)
            {
                return NotFound("No banks found.");
            }

            return Ok(banks);
        }
        /// <summary>
        /// Recupera un banco específico por su ID después de actualizar la base de datos con nuevos bancos.
        /// </summary>
        /// <param name="id">El identificador del banco a recuperar.</param>
        /// <returns>El objeto Bank correspondiente al ID proporcionado, o un resultado NotFound si no se encuentra.</returns>

        [HttpGet("{id}")]
        public async Task<ActionResult<Bank>> FetchAndStoreNewBanks(int id)
        {
            await _bankService.FetchAndStoreNewBanksAsync();
            var bankInfo = await _context.Banks.FindAsync(id);

            if (bankInfo == null)
            {
                return NotFound();
            }

            return bankInfo;
        }
    }
}
