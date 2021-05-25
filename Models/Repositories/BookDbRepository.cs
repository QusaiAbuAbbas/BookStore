using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class BookDbRepository : IBookStoreRepository<Book>,IDisposable
    {
        private readonly BookStoreDbContext _db;
        public BookDbRepository(BookStoreDbContext db)
        {
            this._db = db;
        }
        public void Add(Book entity)
        {
            //entity.Id = books.Max(b => b.Id) + 1;
            _db.Books.Add(entity);
            Commit();
        }

        public void Delete(int id)
        {
            var book = Find(id);
            _db.Books.Remove(book);
            Commit();
        }

        public Book Find(int id)
        {
            var book = _db.Books.Include(a => a.Author).SingleOrDefault(b => b.Id == id);
            return book;
        }

        public IList<Book> List()
        {
            return _db.Books.Include(a => a.Author).ToList();
        }

        public void Update(int id, Book newBook)
        {
            _db.Update(newBook);
            Commit();
        }

        public List<Book> Search(string kw)
        {
            var result = _db.Books.Include(x => x.Author)
                .Where(b => b.Title.Contains(kw)
                         || b.Description.Contains(kw)
                         || b.Author.FullName.Contains(kw)).ToList();
            return result;
        }
        private void Commit()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

       
    }
}
