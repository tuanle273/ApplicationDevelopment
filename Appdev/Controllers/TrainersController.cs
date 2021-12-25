using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Appdev.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Trainer")]
    public class TrainersController : Controller
    {
        private ApplicationDbContext _db;
        private static int courseId;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public TrainersController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Trainers
        public ActionResult Index()
        {
            string userIdCurrentLogin = string.Empty;
            var claimsIdentity = User.Identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                var userIdClaim = claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    userIdCurrentLogin = userIdClaim.Value;
                }
            }

            return View(_db.Assigns.Where(c=> c.TrainerId == userIdCurrentLogin).Include(c=>c.Course).ToList());
        }

        public ActionResult Detail(int id)
        {
            var listOfTrainee = _db.Enrolls.Where(c => c.CourseId == id).Include(c => c.Trainee).ToList();
            return View(listOfTrainee);
        }
    }
}