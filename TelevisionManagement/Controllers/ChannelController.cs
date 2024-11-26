using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
using System.Web.Mvc;
using TelevisionManagement.Auth;
using TelevisionManagement.DTOs;
using TelevisionManagement.EF;

namespace TelevisionManagement.Controllers
{
    [Logged]
    public class ChannelController : Controller
    {
        TelevisionEntities db = new TelevisionEntities();
        public static Channel Convert(ChannelDTO c)
        {
            return new Channel
            {
                ChannelId = c.ChannelId,
                ChannelName = c.ChannelName,
                EstablishedYear = c.EstablishedYear,
                Country = c.Country
            };
        }

        public static ChannelDTO Convert(Channel c)
        {
            return new ChannelDTO
            {
                ChannelId = c.ChannelId,
                ChannelName = c.ChannelName,
                EstablishedYear = c.EstablishedYear,
                Country = c.Country
            };
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Channel());
        }
        
        [HttpPost]
        public ActionResult Create(ChannelDTO c)
        {
            if (ModelState.IsValid)
            { 
               
                if (db.Channels.Any(ch => ch.ChannelName == c.ChannelName))
                {
                    
                    ModelState.AddModelError("ChannelName", "Channel Name already exists. Please choose another name.");
                    return View(c); 
                }
                db.Channels.Add(Convert(c)); 
                db.SaveChanges();
                return RedirectToAction("List"); 
            }

            return View(c);
        }
        
        public static List<ChannelDTO> Convert(List<Channel> data)
        {
            var list = new List<ChannelDTO>();
            foreach (var c in data)
            {
                list.Add(Convert(c));
            }
            return list;
        }
       
        public ActionResult List()
        {
            var data = db.Channels.ToList();
            return View(Convert(data));
        }
        [AdminAccess]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var exobj = db.Channels.Find(id);
            return View(Convert(exobj));
        }
        [HttpPost]
        public ActionResult Edit(ChannelDTO c)
        {
            var exobj = db.Channels.Find(c.ChannelId);
            db.Entry(exobj).CurrentValues.SetValues(c);
            db.SaveChanges();
            return RedirectToAction("List");
        }
        [AdminAccess]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var exobj = db.Channels.Find(id);
            return View(Convert(exobj));
        }
        
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var exobj = db.Channels.Find(Id);
                db.Channels.Remove(exobj);
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }
        public ActionResult Details(int id)
        {
          
            var channel = (from c in db.Channels.Include("Programs") 
                           where c.ChannelId == id
                           select c).SingleOrDefault();

            
            if (channel == null)
            {
                return HttpNotFound();
            }

           
            var programs = channel.Programs; 

            return View(Convert(channel));
        }


    }
}