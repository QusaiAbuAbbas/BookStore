using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStore.Controllers
{
    public class AuthorController : Controller
    {
        private IBookStoreRepository<Author> authorRepoaitory;

        public AuthorController(IBookStoreRepository<Author> authorRepository)
        {
            this.authorRepoaitory = authorRepository;
            if(TempData != null) TempData.Clear();
        }
        // GET: Author
        public ActionResult Index()
        {
            var authors = authorRepoaitory.List();
            return View(authors);
        }

        // GET: Author/Details/5
        public ActionResult Details(int id)
        {
            var author = authorRepoaitory.Find(id);
            return View(author);
        }

        // GET: Author/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Author/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    authorRepoaitory.Add(author);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        // GET: Author/Edit/5
        public ActionResult Edit(int id)
        {
            var author = authorRepoaitory.Find(id);
            return View(author);
        }

        // POST: Author/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Author author)
        {
            try
            {
                authorRepoaitory.Update(id, author);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        // GET: Author/Delete/5
        public ActionResult Delete(int id)
        {
            var author = authorRepoaitory.Find(id);
            return View(author);
        }

        // POST: Author/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Author author)
        {
            try
            {
                authorRepoaitory.Delete(id);
                if(authorRepoaitory.Find(id) != null)
                {
                    TempData["DeletaionFaild"] = "Please delete the books referenced before delete the author!";
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}