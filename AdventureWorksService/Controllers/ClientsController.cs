using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureWorksService.Models;
using AdventureWorksService.Data;

namespace AdventureWorksService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly AdventureWorksContext _context;

        public ClientsController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, DTO.ClientUpdate clientUpdateDTO)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            // Оновлення полів клієнта згідно з даними DTO
            client.FirstName = clientUpdateDTO.FirstName;
            client.LastName = clientUpdateDTO.LastName;
            // Оновіть інші поля за потреби

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(DTO.ClientCreate clientDto)
        {
            // Перетворення DTO на сутність бази даних
            var client = new Client
            {
                // Припускаємо, що ClientId генерується базою даних і тут його вказувати не потрібно
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                // Додайте інші поля за необхідності
            };

            client.ClientId = 0;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.ClientId }, client);
        }


        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Client>> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return client;
        }

        private bool ClientExists(int id)
        {
            return _context.Clients.Any(e => e.ClientId == id);
        }
    }
}

