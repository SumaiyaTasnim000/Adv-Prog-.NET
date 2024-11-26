using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TelevisionManagement.DTOs;
using TelevisionManagement.EF;

namespace TelevisionManagement.Controllers
{
    public class LoginController : Controller
    {
        TelevisionEntities db = new TelevisionEntities();
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(LoginDTO log)
        {
            //
            var user = (from u in db.Users
                        where u.UName.Equals(log.UName) &&
                        u.Password.Equals(log.Password)
                        select u).SingleOrDefault();
            if (user != null)
            {
                Session["user"] = user; //boxing
                return RedirectToAction("List", "Program");
            }
            TempData["Msg"] = "User Not Found";
            return View();
        }
    }
}