using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdventureWorksService.Data;
using AdventureWorksService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorksService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AdventureWorksContext _context;

        public TicketsController(AdventureWorksContext context)
        {
            _context = context;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return await _context.Tickets.ToListAsync();
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.Include(t => t.Bus).Include(t => t.Client).FirstOrDefaultAsync(t => t.TicketId == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(DTO.TicketCreate ticketCreateDto)
        {
            var ticket = new Ticket
            {
                ClientId = ticketCreateDto.ClientId,
                BusId = ticketCreateDto.BusId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }

        // PUT: api/Tickets/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, DTO.TicketUpdate ticketUpdateDto)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ticket.ClientId = ticketUpdateDto.ClientId;
            ticket.BusId = ticketUpdateDto.BusId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
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

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
