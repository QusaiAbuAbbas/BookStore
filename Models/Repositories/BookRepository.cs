﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookRepository : IBookStoreRepository<Book>
        
                
    {
        List<Book> books;
        public BookRepository()
        {
            var noValue = "No Value";
            books = new List<Book>()
            {
                new Book
                {
                    Id = 1,
                    Title = "C# Course",
                    Description = noValue,
                    ImageURL = "20190811_163311.jpg",
                    Author = new Author{Id = 1}
                },
                 new Book
                {
                    Id = 2,
                    Title = ".Net MVC Course",
                    Description = noValue,
                    ImageURL = "20190811_163311.jpg",
                    Author = new Author{Id = 2}

                },
                  new Book
                {
                    Id = 3,
                    Title = ".Net Core Course",
                    Description = noValue,
                    ImageURL="20190811_163311.jpg",
                    Author = new Author{Id = 3}
                }
            };
        }
        public void Add(Book entity)
        {
            entity.Id = books.Max(b => b.Id) + 1;
            books.Add(entity);
        }

        public void Delete(int id)
        {
            var book = Find(id);
            books.Remove(book);
        }

        public Book Find(int id)
        {
            var book = books.SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return books;
        }

        public List<Book> Search(string kw)
        {
            return books.Where(x => x.Title.Contains(kw)).ToList();

        }

        public void Update(int id, Book newBook)
        {
            var book = Find(id);
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Author = newBook.Author;
            book.ImageURL = newBook.ImageURL;

        }
    }
}
