using Editora.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Editora.Domain.EntitiesInterfaces
{
    public interface IBook
    {
        public Task<IEnumerable<Book>> GetAllBooksAsync();
        public Task<Book> GetBookAsync(int id);
        public Task<Book> CreateBookAsync(Book book);
        public Task<Book> UpdateBookAsync(int id, Book book);
        public Task<Book> DeleteBookAsync(int id);
    }
}
