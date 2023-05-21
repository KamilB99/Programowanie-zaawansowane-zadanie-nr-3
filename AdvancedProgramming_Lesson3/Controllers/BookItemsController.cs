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
    public class BookItemsController : ControllerBase
    {
        private readonly BookContext _context;

        public BookItemsController(BookContext context)
        {
            _context = context;
        }

        // GET: api/BookItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookItemDTO>>> GetBookItems()
        {
            return await _context.BookItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/BookItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BookItemDTO>> GetBookItem(long id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(bookItem);
        }

        [HttpPost]
        [Route("UpdateBookItem")]
        public async Task<ActionResult<BookItemDTO>> UpdateBookItem(BookItemDTO bookItemDTO)
        {
            var bookItem = await _context.BookItems.FindAsync(bookItemDTO.Id);
            if (bookItem == null)
            {
                return NotFound();
            }
            bookItem.Title = bookItemDTO.Title;
            bookItem.Author = bookItemDTO.Author;
            bookItem.Genre = bookItemDTO.Genre;
            bookItem.ISBN = bookItemDTO.ISBN;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!BookItemExists(bookItemDTO.Id))
            {
                return NotFound();
            }

            return CreatedAtAction(
                nameof(UpdateBookItem),
                new { id = bookItem.Id },
                ItemToDTO(bookItem));
        }

        [HttpPost]
        [Route("CreateBookItem")]
        public async Task<ActionResult<BookItemDTO>> CreateBookItem(BookItemDTO bookItemDTO)
        {
            var bookItem = new BookItem
            {
                Title = bookItemDTO.Title,
                Author = bookItemDTO.Author,
                Genre = bookItemDTO.Genre,
                ISBN = bookItemDTO.ISBN
            };

            _context.BookItems.Add(bookItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetBookItem),
                new { id = bookItem.Id },
                ItemToDTO(bookItem));
        }

        // DELETE: api/BookItems/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<BookItem>> DeleteBookItem(long id)
        {
            var bookItem = await _context.BookItems.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }
            _context.BookItems.Remove(bookItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        private bool BookItemExists(long id) =>
            _context.BookItems.Any(e => e.Id == id);

        private static BookItemDTO ItemToDTO(BookItem bookItem) =>
            new BookItemDTO
            {
                Id = bookItem.Id,
                Title = bookItem.Title,
                Author = bookItem.Author,
                Genre = bookItem.Genre,
                ISBN = bookItem.ISBN
            };
    }
}
