using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repos
{
    public class NewsRepo
    {
        NewsContext db;
        public NewsRepo()
        {
            db = new NewsContext();
        }
        public void Create(News s)
        {
            db.News.Add(s);
            db.SaveChanges();

        }
        public List<News> Get()
        {
            return db.News.ToList();
        }
        public News Get(int id)
        {
            return db.News.Find(id);
        }

        public void Update(News s)
        {
            var exobj = Get(s.Id);
            db.Entry(exobj).CurrentValues.SetValues(s);
            db.SaveChanges();
        }


        public void Delete(int id)
        {
            var exobj = Get(id);
            db.News.Remove(exobj);
            db.SaveChanges();
        }

        public List<News> Search(string title, string category, string date)
        {
            var query = db.News.AsQueryable(); 

            if (!string.IsNullOrEmpty(title))
                query = query.Where(n => n.Title.Contains(title));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(n => n.Category.Contains(category));

            if (!string.IsNullOrEmpty(date))
                query = query.Where(n => n.Date == date); 

            return query.ToList();
        }

    }
}