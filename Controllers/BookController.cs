using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.Controllers
{

    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookStoreRepository,
                              IBookStoreRepository<Author> authorRepository,
                              IWebHostEnvironment hosting)
        {
            this.bookRepository = bookStoreRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: Book
        public ActionResult Index()
        {
            var books = bookRepository.List();
            return View(books);
        }

        // GET: Book/Details/5
        public ActionResult Details(int id)
        {
            var book = BookFind(id);
            return View(book);
        }

        private Book BookFind(int id)
        {
            return bookRepository.Find(id);
        }

        // GET: Book/Create
        public ActionResult Create()
        {
            var model = new BookAuthorViewModel
            {
                Author = FillSelectList()
            };
            return View(model);
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string fileName = UploadFile(model.File) ?? string.Empty;
                    
                    if (model.AuthorId == -1)
                    {
                        ViewBag.Message = "Please select an auther from the list!";
                        return View(GetAllAuthors());
                    }
                    var author = AuthorFind(model.AuthorId);
                    Book book = new Book
                    {
                        Id = model.BookId,
                        Title = model.Title,
                        Description = model.Description,
                        Author = author,
                        ImageURL = fileName
                    };
                    bookRepository.Add(book);

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "You have to fill all the required fields!");
                return View(GetAllAuthors());
            }

        }

        // GET: Book/Edit/5
        public ActionResult Edit(int id)
        {
            var book = BookFind(id);
            var authorId = book.Author != null ? book.Author.Id : book.Author.Id = 0;
            var viewModel = new BookAuthorViewModel
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = authorId,
                Author = authorRepository.List().ToList(),
                ImageURL = book.ImageURL

            };
            return View(viewModel);
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel viewModel)
        {
            try
            {
                string fileName = UploadFile(viewModel.File,viewModel.ImageURL);          

                var author = AuthorFind(viewModel.AuthorId);
                Book book = new Book
                {
                    Id = viewModel.BookId,
                    Title = viewModel.Title,
                    Description = viewModel.Description,
                    Author = author,
                    ImageURL = fileName
                };
                bookRepository.Update(viewModel.BookId, book);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        private Author AuthorFind(int authorId)
        {
            return authorRepository.Find(authorId);
        }

        // GET: Book/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "please select an author" });
            return authors;
        }

        BookAuthorViewModel GetAllAuthors()
        {
            var vmodel = new BookAuthorViewModel
            {
                Author = FillSelectList()
            };
            return vmodel;
        }

        string UploadFile(IFormFile file)
        {
            if (file != null)
            {
                // to get the path wwwRoot/Uploads
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                // to combain full path wwwRoot/Uploads/fileName
                string fullPath = Path.Combine(uploads, file.FileName);
                // to save file
                file.CopyTo(new FileStream(fullPath, FileMode.Create));
                return file.FileName;
            }
            return null;
        }
        string UploadFile(IFormFile file, string imageURL)
        {
            if (file != null)
            {
                // to get the path wwwRoot/Uploads
                string uploads = Path.Combine(hosting.WebRootPath, "Uploads");
                // to combain full path wwwRoot/Uploads/fileName
                string newPath = Path.Combine(uploads, file.FileName);
                string oldPath = Path.Combine(uploads, imageURL);
                // to make sure the new path not same the old path
                if (CheckIfMatch(newPath,oldPath))
                {
                    // Delete the old file
                    System.IO.File.Delete(oldPath);
                    // to save new file
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }
                return file.FileName;
            }
            return imageURL;
        }

        private bool CheckIfMatch(string newPath, string oldPath)
        {
            if (newPath == oldPath)
                return false;
            return true;
        }

        public ActionResult Search(string kw)
        {
            var result = bookRepository.Search(kw);
            return View("Index",result);
        }
    }
}