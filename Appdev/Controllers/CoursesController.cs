using Appdev.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CoursesController : Controller
    {
        private ApplicationDbContext _db;
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public CoursesController()
        {
            _db = new ApplicationDbContext();
        }


        public ActionResult Index(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return View(_db.Courses.Include(t => t.Category).ToList());
            }
            return View(_db.Courses.Where(t => t.Name.Contains(name)).Include(t => t.Category).ToList());
        }


        public ActionResult Create()
        {
            var selectedcategorylist = new CourseCategoryViewModel()
            {
                Categories = _db.categories.ToList()
            };
            return View(selectedcategorylist);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseCategoryViewModel model)
        {
            var IfCourseExist = _db.Courses.SingleOrDefault(t => t.Name == model.Course.Name);
            if (IfCourseExist != null)
            {
                var selectedcategorylist = new CourseCategoryViewModel()
                {
                    Categories = _db.categories.ToList()
                };
              
                return View(selectedcategorylist);
            }
            else
            {
                _db.Courses.Add(model.Course);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

        }

        public ActionResult Edit(int Id)
        {
            var modelInfo = new CourseCategoryViewModel()
            {
                Course = _db.Courses.SingleOrDefault(t => t.Id == Id),
                Categories = _db.categories.ToList()
            };
            return View(modelInfo);
        }

        [HttpPost]
        public ActionResult Edit(CourseCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var modelInfo = new CourseCategoryViewModel()
                {
                    Course = model.Course,
                    Categories = _db.categories.ToList()
                };
                return View(modelInfo);
            }
            var Coursedb = _db.Courses.SingleOrDefault(c => c.Id == model.Course.Id);
            if (Coursedb == null)
            {
                return HttpNotFound();
            }
            Coursedb.Name = model.Course.Name;
            Coursedb.Description = model.Course.Description;
            Coursedb.CategoryId = model.Course.CategoryId;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            var Coursedb = _db.Courses.SingleOrDefault(c => c.Id == Id);
            _db.Courses.Remove(Coursedb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}