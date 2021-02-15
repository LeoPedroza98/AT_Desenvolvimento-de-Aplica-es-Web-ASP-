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
    public class AuthorController : Controller
    {
        private readonly ApiClient apiClient;

        public AuthorController()
        {
            apiClient = new ApiClient();
        }
        // GET: AuthorController
        [Authorize]
        public async Task<ActionResult> Index()
        {
            List<Author> authors = new List<Author>();
            HttpResponseMessage response = await apiClient.Client.GetAsync("api/AuthorAPI");
            if (response.IsSuccessStatusCode)
            {
                var results = response.Content.ReadAsStringAsync().Result;
                authors = JsonConvert.DeserializeObject<List<Author>>(results);
            }
            return View(authors);
        }

        // GET: AuthorController/Details/5
        [Authorize]
        public async Task<ActionResult> Details(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Author author;
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/AuthorAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                author = JsonConvert.DeserializeObject<Author>(result);
                return View(author);
            }
            return NotFound();
        }

        // GET: AuthorController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Surname,Birthdate,Email")] Author author)
        {
            if (ModelState.IsValid)
            {
                var post = apiClient.Client.PostAsJsonAsync<Author>("api/AuthorAPI", author);
                post.Wait();

                var result = post.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(author);
        }

        // GET: AuthorController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Author author = new Author();
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/AuthorAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                author = JsonConvert.DeserializeObject<Author>(result);
                return View(author);
            }
            return NotFound();
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, [Bind("Id,Name,Surname,Birthdate,Email")] Author author)
        {
            if (id != author.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var post = apiClient.Client.PutAsJsonAsync<Author>($"api/AuthorAPI/{author.ID}", author);
                post.Wait();

                var result = post.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(author);
        }

        // GET: AuthorController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int?id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Author author;
            HttpResponseMessage response = await apiClient.Client.GetAsync($"api/AuthorAPI/{id}");
            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                author = JsonConvert.DeserializeObject<Author>(result);
                return View(author);
            }
            return NotFound();
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            await apiClient.Client.DeleteAsync($"api/AuthorAPI/{id}");
            return RedirectToAction("Index");
        }
    }
}
