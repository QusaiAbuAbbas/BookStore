using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Repositories
{
    public class AuthorDbRepository : IBookStoreRepository<Author>
    {
        private readonly BookStoreDbContext db;
        public AuthorDbRepository(BookStoreDbContext _db)
        {
            db = _db;
        }
        public void Add(Author entity)
        {
           // entity.Id = db.Authors.Max(b => b.Id) + 1;
            db.Authors.Add(entity);
            Commit();
        }

        public void Delete(int id)
        {
            var author = Find(id);
            db.Authors.Remove(author);
            Commit();
        }

        public Author Find(int id)
        {
            var author = db.Authors.SingleOrDefault(x => x.Id == id);
            return author;
        }

        public IList<Author> List()
        {
            return db.Authors.ToList();
        }

        public List<Author> Search(string kw)
        {
            return db.Authors.Where(x => x.FullName.Contains(kw)).ToList();
        }

        public void Update(int id, Author newAuthor)
        {
            db.Update(newAuthor);
           Commit();
        }

        private void Commit()
        {
            db.SaveChanges();
        }
    }
}

