using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{

    public class WeddingController : Controller
    {
        private WeddingContext _context;

        public WeddingController(WeddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CurrentUser = HttpContext.Session.GetInt32("CurrentUserID");
            ViewBag.Weddings = _context.Weddings.Include(e => e.GuestLists).ToList();

            return View("Dashboard");
        }

        [HttpPost]
        [Route("rsvp")]
        public IActionResult RSVP(string rsvpAction, int WeddingID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            int? CurrentUser = HttpContext.Session.GetInt32("CurrentUserID");
            if (rsvpAction == "RSVP")
            {
                WeddingGuest newGuest = new WeddingGuest
                {
                    WeddingAttendeeID = (int)CurrentUser,
                    WeddingEventID = WeddingID
                };
                _context.WeddingGuests.Add(newGuest);
            }
            else if (rsvpAction == "Un-RSVP")
            {
                var undo = _context.WeddingGuests.Where(e => e.WeddingAttendeeID == (int)CurrentUser).Where(e => e.WeddingEventID == WeddingID).SingleOrDefault();
                _context.WeddingGuests.Remove(undo);
            }
            else if (rsvpAction == "Delete")
            {
                var deleteWedding = _context.Weddings.Where(e => e.WeddingID == WeddingID).SingleOrDefault();
                _context.Weddings.Remove(deleteWedding);
            }

            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("plan")]
        public IActionResult Plan()
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Plan");
        }

        [HttpPost]
        [Route("plan/create")]
        public IActionResult Create(WeddingPlanViewModel model)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }

            int creatorID = (int)HttpContext.Session.GetInt32("CurrentUserID");
            if (ModelState.IsValid)
            {
                Wedding newWedding = new Wedding
                {
                    Wedder1 = model.Wedder1,
                    Wedder2 = model.Wedder2,
                    Address = model.Address,
                    CreatorID = creatorID,
                    WeddingDate = model.WeddingDate
                };
                _context.Weddings.Add(newWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            return View("Plan");
        }

        [HttpGet]
        [Route("guestlist/{WeddingID}")]
        public IActionResult GuestList(int WeddingID)
        {
            if (!CheckLoggedIn())
            {
                return RedirectToAction("Index", "Home");
            }
            Wedding query = _context.Weddings.Include(w => w.GuestLists).ThenInclude(g => g.WeddingAttendee).Where(r => r.WeddingID  == WeddingID).SingleOrDefault();
            return View(query);
        }

        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public bool CheckLoggedIn()
        {
            if (HttpContext.Session.GetInt32("CurrentUserID") == null)
            {
                return false;
            }
            return true;
        }
    }
}
