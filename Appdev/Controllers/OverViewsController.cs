using Appdev.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class OverViewsController : Controller
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
        public OverViewsController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: OverViews
        public ActionResult SelectCourse()
        {
            return View(_db.Courses.Include(c => c.Category).ToList());
        }

        public ActionResult Overview(int id)
        {
            OverViewViewModel viewModel = new OverViewViewModel()
            {
                TrainerList = _db.Assigns.Where(c => c.CourseId == id).Include(t=>t.Trainer).ToList(),
                TraineeList = _db.Enrolls.Where(c => c.CourseId == id).Include(t => t.Trainee).ToList()
            };
            return View(viewModel);
        }
    }
}