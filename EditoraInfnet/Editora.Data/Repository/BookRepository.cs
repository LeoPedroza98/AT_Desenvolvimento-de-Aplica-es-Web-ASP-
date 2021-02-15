using Editora.Domain.Entities;
using Editora.Domain.EntitiesInterfaces;
using Editora.MVC.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editora.Data.Repository
{
    public class BookRepository : IBook
    {
        private readonly ApplicationDbContext applicationDb;

        public BookRepository(ApplicationDbContext context)
        {
            applicationDb = context;
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            var _book = await applicationDb.Books.AddAsync(book);
            await applicationDb.SaveChangesAsync();
            return _book.Entity;
        }

        public async Task<Book> DeleteBookAsync(int id)
        {
            var book = await applicationDb.Books.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            applicationDb.Books.Remove(book);
            await applicationDb.SaveChangesAsync();

            return book;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            var books = await applicationDb.Books.ToListAsync();
            return books;
        }

        public async Task<Book> GetBookAsync(int id)
        {
            var book = await applicationDb.Books.FindAsync(id);
            return book;
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            applicationDb.Entry(book).State = EntityState.Modified;

            try
            {
                await applicationDb.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return book;
        }

        private bool BookExists(int id)
        {
            return applicationDb.Books.Any(e => e.ID == id);
        }
    }
}
