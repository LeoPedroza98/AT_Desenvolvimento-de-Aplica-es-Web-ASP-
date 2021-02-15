using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Editora.Domain.Entities
{
    public class Book
    {

        [Key]
        public int ID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Year of Release")]
        public int Year { get; set; }

        [Required]
        public string ISBN { get; set; }

        public ICollection<AuthorBook> AuthorBooks { get; set; }

        [NotMapped]
        public Author Author { get; set; }
    }
}
