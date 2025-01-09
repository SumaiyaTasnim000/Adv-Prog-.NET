using BLL.DTOs;
using BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NewsManagement.Controllers
{
    public class NewsController : ApiController
    {
        [HttpGet]
        [Route("api/news/all")]
        public HttpResponseMessage Get()
        {
            var data = NewsService.Get();
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }
        [HttpPost]
        [Route("api/news/create")]
        public HttpResponseMessage Create(NewsDTO s)
        {
            NewsService.Create(s);
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpPut]
        [Route("api/news/update/{id}")]
        public HttpResponseMessage Update(int id, NewsDTO s)
        {
            NewsService.Update(s);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpDelete]
        [Route("api/news/delete/{id}")]
        public HttpResponseMessage Delete(int id)
        {
            NewsService.Delete(id);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpGet]
        [Route("api/news/search")]
        public HttpResponseMessage Search(string title = "", string category = "", string date = "")
        {
            var data = NewsService.Search(title, category, date);
            return Request.CreateResponse(HttpStatusCode.OK, data);
        }

    }
}
