using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureWorksService.Data;
using AdventureWorksService.Models;

namespace AdventureWorksService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly AdventureWorksContext _context;

        public BusesController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: api/Buses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bus>>> GetBuses()
        {
            return await _context.Buses.ToListAsync();
        }

        // GET: api/Buses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bus>> GetBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);

            if (bus == null)
            {
                return NotFound();
            }

            return bus;
        }

        // POST: api/Buses
        [HttpPost]
        public async Task<ActionResult<Bus>> PostBus(DTO.BusCreate busCreateDto)
        {
            var bus = new Bus
            {
                LicensePlate = busCreateDto.LicensePlate,
                Model = busCreateDto.Model,
                SeatsAvailable = busCreateDto.SeatsAvailable
            };

            _context.Buses.Add(bus);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBus), new { id = bus.BusId }, bus);
        }


        // PUT: api/Buses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBus(int id, DTO.BusUpdate busUpdateDto)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }

            bus.LicensePlate = busUpdateDto.LicensePlate;
            bus.Model = busUpdateDto.Model;
            bus.SeatsAvailable = busUpdateDto.SeatsAvailable;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusExists(id))
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


        // DELETE: api/Buses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBus(int id)
        {
            var bus = await _context.Buses.FindAsync(id);
            if (bus == null)
            {
                return NotFound();
            }

            _context.Buses.Remove(bus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BusExists(int id)
        {
            return _context.Buses.Any(e => e.BusId == id);
        }
    }
}
