using System;
using System.Collections.Generic;
using System.Text;

namespace Editora.Domain.Entities
{
    public class AuthorBook
    {
        public int BookID { get; set; }

        public Book Book { get; set; }

        public int AuthorID { get; set; }

        public Author Author { get; set; }
    }
}
