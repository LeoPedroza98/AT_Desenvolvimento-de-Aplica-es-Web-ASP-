using System;
using System.Collections.Generic;
using System.Text;
using Editora.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Editora.MVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EditoraInfnet-DB;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Author>().ToTable("Author");
            builder.Entity<Book>().ToTable("Book");
            builder.Entity<AuthorBook>().ToTable("AuthorBook");

            builder.Entity<AuthorBook>().HasKey(ba => new { ba.BookID, ba.AuthorID });

            builder.Entity<AuthorBook>()
                .HasOne(ba => ba.Book)
                .WithMany(b => b.AuthorBooks)
                .HasForeignKey(ba => ba.BookID)
                .IsRequired();

            builder.Entity<AuthorBook>()
                .HasOne(ba => ba.Author)
                .WithMany(a => a.AuthorBooks)
                .HasForeignKey(ba => ba.AuthorID)
                .IsRequired();
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
    }
}
