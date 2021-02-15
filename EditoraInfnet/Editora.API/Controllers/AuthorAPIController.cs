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
    public class AuthorAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthorRepository aRepository;

        public AuthorAPIController(ApplicationDbContext context,AuthorRepository authorRepository)
        {
            _context = context;
            aRepository = authorRepository;
        }

        // GET: api/AuthorAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
        {
            return Ok(await aRepository.GetAllAuthorsAsync());
        }

        // GET: api/AuthorAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            var author = await aRepository.GetAuthorAsync(id);

            if (author == null)
            {
                return NotFound();
            }

            return author;
        }

        // PUT: api/AuthorAPI/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAuthor(int id, Author author)
        {
            if (id != author.ID)
            {
                return BadRequest();
            }

            var rs = await aRepository.UpdateAuthorAsync(id,author);
            if (rs == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // POST: api/AuthorAPI
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Author>> PostAuthor(Author author)
        {

            author = await aRepository.CreateAuthorAsync(author);

            return CreatedAtAction("GetAuthor", new { id = author.ID }, author);
        }

        // DELETE: api/AuthorAPI/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            var author = await aRepository.DeleteAuthorAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return author;
        }
    }
}
