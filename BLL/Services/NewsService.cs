using AutoMapper;
using BLL.DTOs;
using DAL;
using DAL.EF;
using DAL.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NewsService
    {
        private static readonly NewsRepo repo = new NewsRepo();
        public static List<NewsDTO> Get()
            {
                var repo = new NewsRepo();
                var data = repo.Get();

                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<News, NewsDTO>();
                });
                var mapper = new Mapper(config);
                var ret = mapper.Map<List<NewsDTO>>(data);

                return ret;
            }
            public static void Create(NewsDTO s)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<NewsDTO, News>();
                });
                var mapper = new Mapper(config);
                var ret = mapper.Map<News>(s);
                var repo = new NewsRepo();
                repo.Create(ret);

            }
        public static void Update(NewsDTO newsDto)
        {
            repo.Update(new News
            {
                Id = newsDto.Id,
                Title = newsDto.Title,
                Category = newsDto.Category,
                Date = newsDto.Date
            });
        }

        public static void Delete(int id)
        {
            repo.Delete(id);
        }

        public static List<NewsDTO> Search(string title, string category, string date)
        {
            var newsList = repo.Search(title, category, date);
            return newsList.Select(n => new NewsDTO
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                Date = n.Date
            }).ToList();
        }




    }
}
