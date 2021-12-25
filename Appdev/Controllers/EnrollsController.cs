using Appdev.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class EnrollsController : Controller
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
        public EnrollsController()
        {
            _db = new ApplicationDbContext();
        }
        // GET: Enrollments
        public ActionResult SelectCourse()
        {
            return View(_db.Courses.Include(c=>c.Category).ToList());
        }

        public ActionResult SelectTrainee(int? id)
        {
            if (id != null)
            {
                courseId = id.Value;
            }
            return View(_db.Users.OfType<Trainee>().ToList());
        }

        public ActionResult Enrollment(string Id)
        {
            if (Id == null)
            {
                ViewBag.message = "Error when enroll";
                return RedirectToAction("SelectCourse");
            }
            Enroll enrollment = new Enroll()
            {
                TraineeId = Id,
                CourseId = courseId
            };
            var enrollExist = _db.Enrolls.Where(c => c.CourseId == courseId && c.TraineeId == Id);
            if (enrollExist.Any())
            {
                ViewBag.message = "Error when enroll";
                return RedirectToAction("SelectTrainee");
            }
            _db.Enrolls.Add(enrollment);
            _db.SaveChanges();
            ViewBag.message = "Enroll Successfully";
            return RedirectToAction("SelectCourse");
        }

        public ActionResult DeleteEnroll(string id)
        {
            if (id == null)
            {
                ViewBag.message = "Error when Delete";
                return RedirectToAction("SelectCourse");
            }
            var enrollment = _db.Enrolls.Where(e => e.CourseId == courseId && e.TraineeId == id).FirstOrDefault();
            _db.Enrolls.Remove(enrollment);
            _db.SaveChanges();
            ViewBag.message = "Delete Successfully";
            return RedirectToAction("SelectCourse");
        }
    }
}