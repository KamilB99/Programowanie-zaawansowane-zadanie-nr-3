using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedProgramming_Lesson3.Models;

namespace AdvancedProgramming_Lesson3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OsobaItemsController : ControllerBase
    {
        private readonly OsobaContext _context;

        public OsobaItemsController(OsobaContext context)
        {
            _context = context;
        }

        // GET: api/OsobaItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OsobaItemDTO>>> GetOsobaItems()
        {
            return await _context.OsobaItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/OsobaItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OsobaItemDTO>> GetOsobaItem(long id)
        {
            var osobaItem = await _context.OsobaItems.FindAsync(id);
            if (osobaItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(osobaItem);
        }

        [HttpPost]
        [Route("UpdateOsobaItem")]
        public async Task<ActionResult<OsobaItemDTO>> UpdateOsobaItem(OsobaItemDTO osobaItemDTO)
        {
            var osobaItem = await _context.OsobaItems.FindAsync(osobaItemDTO.Id);
            if (osobaItem == null)
            {
                return NotFound();
            }
            osobaItem.Imie = osobaItemDTO.Imie;
            osobaItem.Nazwisko = osobaItemDTO.Nazwisko;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!OsobaItemExists(osobaItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateOsobaItem),
                new { id = osobaItem.Id },
                ItemToDTO(osobaItem));
        }

        [HttpPost]
        [Route("CreateOsobaItem")]
        public async Task<ActionResult<OsobaItemDTO>> CreateOsobaItem(OsobaItemDTO osobaItemDTO)
        {
            var osobaItem = new OsobaItem
            {
                Imie = osobaItemDTO.Imie,
                Nazwisko = osobaItemDTO.Nazwisko
            };

            _context.OsobaItems.Add(osobaItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetOsobaItem),
                new { id = osobaItem.Id },
                ItemToDTO(osobaItem));
        }

        // DELETE: api/OsobaItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<OsobaItem>> DeleteOsobaItem(long id)
        {
            var osobaItem = await _context.OsobaItems.FindAsync(id);
            if (osobaItem == null)
            {
                return NotFound();
            }
            _context.OsobaItems.Remove(osobaItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool OsobaItemExists(long id) =>
            _context.OsobaItems.Any(e => e.Id == id);

        private static OsobaItemDTO ItemToDTO(OsobaItem osobaItem) =>
            new OsobaItemDTO
            {
                Id = osobaItem.Id,
                Imie = osobaItem.Imie,
                Nazwisko = osobaItem.Nazwisko
            };
    }
}