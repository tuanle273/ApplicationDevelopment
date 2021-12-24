using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ApplicationDevelopment.Models;
using ApplicationDevelopment.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;


namespace ApplicationDevelopment.Controllers
{
    [Authorize(Roles = "Staff")]
    public class AssignmentController : Controller
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
        public AssignmentController()
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
                ViewBag.message = "Error when Assignment";
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
                ViewBag.message = "Error when enroll";
                return RedirectToAction("SelectTrainer");
            }
            _db.Assigns.Add(assign);
            _db.SaveChanges();
            ViewBag.message = "Assignment Successfully";
            return RedirectToAction("SelectCourse");
        }

        public ActionResult DeleteAssignment(string id)
        {
            if (id == null)
            {
                ViewBag.message = "Error when Delete";
                return RedirectToAction("SelectCourse");
            }
            var assignment = _db.Assigns.Where(e => e.CourseId == courseId && e.TrainerId == id).FirstOrDefault();
            _db.Assigns.Remove(assignment);
            _db.SaveChanges();
            ViewBag.message = "Delete Successfully";
            return RedirectToAction("SelectCourse");
        }
        public ActionResult ChangeCourse(string id)
        {

            var changeCourseViewmodel = new ChangeCourseViewmodel()
            {
                CourseList = _db.Courses.Where(c => c.Id != courseId),
                UserId = id
            };
            return View(changeCourseViewmodel);
        }
        [HttpPost]
        public ActionResult ChangeCourse(ChangeCourseViewmodel model)
        {
            var assigndb = _db.Assigns.Where(c => c.CourseId == courseId && c.TrainerId == model.UserId).FirstOrDefault();
            if (assigndb == null)
            {
                ViewBag.message = "Error non-existed course assigned";
            }
            var assignnewcourseExist = _db.Assigns.Where(c => c.CourseId == courseId && c.TrainerId == model.UserId);
            if (!assignnewcourseExist.Any())
            {
                ViewBag.message = "Error  Course already change";
                return RedirectToAction("SelectCourse");
            }
            assigndb.CourseId = model.Course.Id;
            _db.SaveChanges();
            return RedirectToAction("SelectCourse");
        }
    }
}