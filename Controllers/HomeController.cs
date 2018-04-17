using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers
{

    public class HomeController : Controller
    {
        private WeddingContext _context;

        public HomeController(WeddingContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            bool existing = _context.Users.Any(user => user.Email == model.Email);
            if (ModelState.IsValid && !existing)
            {
                User NewUser = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.Password = Hasher.HashPassword(NewUser, NewUser.Password);

                _context.Users.Add(NewUser);
                _context.SaveChanges();
                var loginUser = _context.Users.SingleOrDefault(User => User.Email == model.Email);

                HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                return RedirectToAction("Dashboard", "Wedding");
            }
            ViewBag.Error = "Email Already Registered!";
            System.Console.WriteLine("Something Went wrong");
            return View("Index");
        }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("loggingIn")]
        public IActionResult LoggingIn(string loginemail, string loginpw)
        {
            PasswordHasher<User> Hasher = new PasswordHasher<User>();

            var loginUser = _context.Users.SingleOrDefault(User => User.Email == loginemail);
            if (loginUser != null)
            {
                var hashedPw = Hasher.VerifyHashedPassword(loginUser, loginUser.Password, loginpw);
                if (hashedPw == PasswordVerificationResult.Success)
                {
                    HttpContext.Session.SetInt32("CurrentUserID", loginUser.UserID);
                    return RedirectToAction("Dashboard", "Wedding");
                }
            }

            ViewBag.Error = "Email address or Password is not matching";
            return View("Index");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
