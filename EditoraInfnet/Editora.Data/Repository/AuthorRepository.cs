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
    public class AuthorRepository : IAuthor
    {
        private readonly ApplicationDbContext applicationDb;

        public AuthorRepository(ApplicationDbContext context)
        {
            applicationDb = context;
        }
        public async Task<Author> CreateAuthorAsync(Author author)
        {
            var _author = await applicationDb.Authors.AddAsync(author);
            await applicationDb.SaveChangesAsync();
            return _author.Entity;
        }

        public async Task<Author> DeleteAuthorAsync(int id)
        {
            var author = await applicationDb.Authors.FindAsync(id);
            if (author == null)
            {
                return null;
            }

            applicationDb.Authors.Remove(author);
            await applicationDb.SaveChangesAsync();

            return author;
        }

        public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
        {
            var authors = await applicationDb.Authors.ToListAsync();
            return authors;
        }

        public async Task<Author> GetAuthorAsync(int id)
        {
            var author = await applicationDb.Authors.FindAsync(id);
            return author;
        }

        public async Task<Author> UpdateAuthorAsync(int id, Author author)
        {
            applicationDb.Entry(author).State = EntityState.Modified;

            try
            {
                await applicationDb.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
            return author;
        }

        private bool AuthorExists(int id)
        {
            return applicationDb.Authors.Any(e => e.ID == id);
        }
    }
}
