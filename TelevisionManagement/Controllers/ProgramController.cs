using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;
using TelevisionManagement.DTOs;
using TelevisionManagement.EF;
using TelevisionManagement.Auth;

namespace TelevisionManagement.Controllers
{
    [Logged]
    public class ProgramController : Controller
    {
        TelevisionEntities db = new TelevisionEntities();
        public static Program Convert(ProgramDTO p)
        {
            return new Program
            {
                ProgramId = p.ProgramId,
                ProgramName = p.ProgramName,
                TRPScore = p.TRPScore,
                ChannelId = p.ChannelId,
                AirTime = p.AirTime
            };
        }

        public static ProgramDTO Convert(Program p)
        {
            return new ProgramDTO
            {
                ProgramId = p.ProgramId,
                ProgramName = p.ProgramName,
                TRPScore = p.TRPScore,
                ChannelId = p.ChannelId,
                AirTime = p.AirTime
            };
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Program());
        }
        [HttpPost]
        public ActionResult Create(ProgramDTO p)
        {
            if (ModelState.IsValid)
            {
                // Check if the ProgramName already exists within the same ChannelId
                if (db.Programs.Any(pr => pr.ProgramName == p.ProgramName && pr.ChannelId == p.ChannelId))
                {
                    ModelState.AddModelError("ProgramName", "Program Name already exists within this channel. Please choose another name.");
                    ViewBag.Channels = db.Channels.ToList(); // Repopulate channels for the dropdown
                    return View(p);
                }

                // Convert DTO to Entity and Save
                db.Programs.Add(Convert(p));
                db.SaveChanges();
                return RedirectToAction("List");
            }

            ViewBag.Channels = db.Channels.ToList(); // Repopulate channels for the dropdown
            return View(p);
        }

        public static List<ProgramDTO> Convert(List<Program> data)
        {
            var list = new List<ProgramDTO>();
            foreach (var p in data)
            {
                list.Add(Convert(p));
            }
            return list;
        }

        public ActionResult List()
        {
            var data = db.Programs.ToList();
            return View(Convert(data));
        }



        [AdminAccess]

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var exobj = db.Programs.Find(id);
            return View(Convert(exobj));
        }

        [HttpPost]
        public ActionResult Edit(ProgramDTO p)
        {
            if (ModelState.IsValid)
            {
                // Check for uniqueness, excluding the current program being edited
                if (db.Programs.Any(pr => pr.ProgramName == p.ProgramName && pr.ChannelId == p.ChannelId && pr.ProgramId != p.ProgramId))
                {
                    ModelState.AddModelError("ProgramName", "Program Name already exists within this channel. Please choose another name.");
                    ViewBag.Channels = db.Channels.ToList(); // Repopulate channels for the dropdown
                    return View(p);
                }

                // Update the entity
                var existingProgram = db.Programs.Find(p.ProgramId);
                if (existingProgram != null)
                {
                    db.Entry(existingProgram).CurrentValues.SetValues(p);
                    db.SaveChanges();
                    return RedirectToAction("List");
                }
            }

            ViewBag.Channels = db.Channels.ToList(); // Repopulate channels for the dropdown
            return View(p);
        }
        [AdminAccess]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var exobj = db.Programs.Find(id);
            return View(Convert(exobj));
        }
        [HttpPost]
        public ActionResult Delete(int Id, string dcsn)
        {
            if (dcsn.Equals("Yes"))
            {
                var exobj = db.Programs.Find(Id);
                db.Programs.Remove(exobj);
                db.SaveChanges();

            }
            return RedirectToAction("List");
        }
        public ActionResult Details(int id)
        {
            // Retrieve the program by its ID, along with the associated channel
            var program = db.Programs
                            .Where(p => p.ProgramId == id)
                            .Include(p => p.Channel) // Include related channel
                            .FirstOrDefault();

            // If program is found, pass it to the view
            return View(program);
        }


        //Search
        public ActionResult Search(string searchTerm)
        {
            // Start with all programs, including related channel data
            var programs = db.Programs.Include(p => p.Channel).AsQueryable();

            // If the search term is provided, filter by Program Name, TRP Score, or Channel Name
            if (!string.IsNullOrEmpty(searchTerm))
            {
                programs = programs.Where(p =>
                    p.ProgramName.Contains(searchTerm) ||             // Search by Program Name
                    p.TRPScore.ToString().Contains(searchTerm) ||     // Search by TRP Score
                    p.Channel.ChannelName.Contains(searchTerm)        // Search by Channel Name
                );
            }

            return View(programs.ToList());
        }


    }
}
