using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Editora.Domain.Entities;
using Editora.MVC.Data;
using Microsoft.AspNetCore.Authorization;

namespace Editora.MVC.Controllers
{
    public class AuthorBooksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorBooksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AuthorBooks
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AuthorBooks.Include(a => a.Author).Include(a => a.Book);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AuthorBooks/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorBook = await _context.AuthorBooks
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (authorBook == null)
            {
                return NotFound();
            }

            return View(authorBook);
        }

        // GET: AuthorBooks/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name");
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "ISBN");
            return View();
        }

        // POST: AuthorBooks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BookID,AuthorID")] AuthorBook authorBook)
        {
            if (ModelState.IsValid)
            {
                _context.Add(authorBook);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", authorBook.AuthorID);
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "ISBN", authorBook.BookID);
            return View(authorBook);
        }

        // GET: AuthorBooks/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorBook = await _context.AuthorBooks.FindAsync(id);
            if (authorBook == null)
            {
                return NotFound();
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", authorBook.AuthorID);
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "ISBN", authorBook.BookID);
            return View(authorBook);
        }

        // POST: AuthorBooks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("BookID,AuthorID")] AuthorBook authorBook)
        {
            if (id != authorBook.BookID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authorBook);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorBookExists(authorBook.BookID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorID"] = new SelectList(_context.Authors, "ID", "Name", authorBook.AuthorID);
            ViewData["BookID"] = new SelectList(_context.Books, "ID", "ISBN", authorBook.BookID);
            return View(authorBook);
        }

        // GET: AuthorBooks/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authorBook = await _context.AuthorBooks
                .Include(a => a.Author)
                .Include(a => a.Book)
                .FirstOrDefaultAsync(m => m.BookID == id);
            if (authorBook == null)
            {
                return NotFound();
            }

            return View(authorBook);
        }

        // POST: AuthorBooks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authorBook = await _context.AuthorBooks.FindAsync(id);
            _context.AuthorBooks.Remove(authorBook);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorBookExists(int id)
        {
            return _context.AuthorBooks.Any(e => e.BookID == id);
        }
    }
}
