using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Editora.Domain.Entities;
using Editora.MVC.ApiClientHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Editora.MVC.Controllers
{
    public class BookController : Controller
    {
        private readonly ApiClient apiClient;

        public BookController()
        {
            apiClient = new ApiClient();
        }
        // GET: BookController
        [Authorize]
        public async Task<ActionResult> Index()
        {
            List<Book> books = new List<Book>();
            HttpResponseMessage response = await apiClient.Client.GetAsync("api/BookAPI");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                books = JsonConvert.DeserializeObject<List<Book>>(results);
            }
            return View(books);
        }

        // GET: BookController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book book;
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/BookAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(result);
                return View(book);
            }
            return NotFound();
        }

        // GET: BookController/Create
        [Authorize]
        public async Task<ActionResult> Create()
        {
            await PopulateAuthorDropdown();
            return View();
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(int Id, string Title, int Year, string ISBN)
        {
            int authorid = int.Parse(Request.Form["author"]);

            Book book = new Book { ID = Id, Title = Title, Year = Year, ISBN = ISBN };

            var postbook = apiClient.Client.PostAsJsonAsync<Book>("api/BookApi", book);
            postbook.Wait();

            if (!postbook.Result.IsSuccessStatusCode)
            {
                await PopulateAuthorDropdown();
                return View(book);
            }

            string bookUri = postbook.Result.Headers.Location.AbsolutePath;
            string bookId = bookUri.Split("/").Last();
            int bookIdParsed = int.Parse(bookId);

            AuthorBook bookAuthor = new AuthorBook { BookID = bookIdParsed, AuthorID = authorid };
            CreateBookAuthor(bookAuthor);

            return RedirectToAction("Index");
        }

        // GET: BookController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book book = new Book();
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/BookAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(result);
                return View(book);
            }
            return NotFound();
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, [Bind("Id,Title,Year,ISBN")] Book book)
        {
            if (id != book.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var post = apiClient.Client.PutAsJsonAsync<Book>($"api/BookAPI/{book.ID}", book);
                post.Wait();

                var result = post.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        // GET: BookController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Book book;
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/BookAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                book = JsonConvert.DeserializeObject<Book>(result);
                return View(book);
            }
            return NotFound();
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            await apiClient.Client.DeleteAsync($"api/BookAPI/{id}");
            return RedirectToAction("Index");
        }
        private async Task PopulateAuthorDropdown()
        {
            List<Author> authors = new List<Author>();
            HttpResponseMessage response = await apiClient.Client.GetAsync("api/AuthorAPI");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                authors = JsonConvert.DeserializeObject<List<Author>>(results);
            }
            ViewData["authors"] = authors;
        }

        private bool CreateBookAuthor(AuthorBook authorBook)
        {
            var postbookauthor = apiClient.Client.PostAsJsonAsync<AuthorBook>("api/BookAuthorAPI", authorBook);
            postbookauthor.Wait();

            var results = postbookauthor.Result;
            if (results.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
