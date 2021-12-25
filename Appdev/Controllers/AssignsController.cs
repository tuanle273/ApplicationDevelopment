using Appdev.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class AssignsController : Controller
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
        public AssignsController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Enrollments
        public ActionResult SelectCourse()
        {
            return View(_db.Courses.Include(t => t.Category).ToList());
        }

        public ActionResult SelectTrainer(int? id)
        {
            if (id != null)
            {
                courseId = id.Value;
            }
            return View(_db.Users.OfType<Trainer>().ToList());
        }

        public ActionResult Assignment(string Id)
        {
            if (Id == null)
            {
                return RedirectToAction("SelectCourse");
            }
            Assign assign = new Assign()
            {
                TrainerId = Id,
                CourseId = courseId
            };
            var assignExist = _db.Assigns.Where(c => c.CourseId == courseId && c.TrainerId == Id);
            if (assignExist.Any())
            {
                return RedirectToAction("SelectTrainer");
            }
            _db.Assigns.Add(assign);
            _db.SaveChanges();
            ViewData["Message"] = "Success: Assign Successfully";
            return RedirectToAction("SelectCourse");
        }

        public ActionResult DeleteAssignment(string id)
        {
            if (id == null)
            {
                return RedirectToAction("SelectCourse");
            }
            var assignment = _db.Assigns.Where(e => e.CourseId == courseId && e.TrainerId == id).FirstOrDefault();
            _db.Assigns.Remove(assignment);
            _db.SaveChanges();
            ViewData["Message"] = "Success: Delete Assign Successfully";
            return RedirectToAction("SelectCourse");
        }
    }
}