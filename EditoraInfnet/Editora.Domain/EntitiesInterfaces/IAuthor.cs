using Editora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Editora.Domain.EntitiesInterfaces
{
    public interface IAuthor
    {
        public Task<IEnumerable<Author>> GetAllAuthorsAsync();
        public Task<Author> GetAuthorAsync(int id);
        public Task<Author> CreateAuthorAsync(Author author);
        public Task<Author> UpdateAuthorAsync(int id, Author author);
        public Task<Author> DeleteAuthorAsync(int id);

    }
}
