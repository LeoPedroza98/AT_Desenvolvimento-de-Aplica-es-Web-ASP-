using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Editora.Domain.Entities;
using Editora.MVC.Data;
using Editora.Data.Repository;

namespace Editora.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly BookRepository bRepository;

        public BookAPIController(ApplicationDbContext context,BookRepository bookRepository)
        {
            _context = context;
            bRepository = bookRepository;
        }

        // GET: api/BookAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return Ok(await bRepository.GetAllBooksAsync());
        }

        // GET: api/BookAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await bRepository.GetBookAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }

        // PUT: api/BookAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.ID)
            {
                return BadRequest();
            }

            var rs = await bRepository.UpdateBookAsync(id, book);
            if (rs == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/BookAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            book = await bRepository.CreateBookAsync(book); 

            return CreatedAtAction("GetBook", new { id = book.ID }, book);
        }

        // DELETE: api/BookAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            var book = await bRepository.DeleteBookAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
    }
}
