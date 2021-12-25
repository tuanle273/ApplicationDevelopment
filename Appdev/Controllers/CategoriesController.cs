using Appdev.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoriesController : Controller
    {
        private ApplicationDbContext _db;

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        public CategoriesController()
        {
            _db = new ApplicationDbContext();
        }


        public ActionResult Index(string cateName)
        {
            if (String.IsNullOrWhiteSpace(cateName))
            {
                return View(_db.categories.ToList());
            }
            return View(_db.categories.Where(t => t.Name.Contains(cateName)).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category c)
        {
            var IfCategoryExist = _db.categories.SingleOrDefault(t => t.Name == c.Name);
            if (IfCategoryExist != null)
            {
             
                return View();
            }
            else
            {
                _db.categories.Add(c);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int Id)
        {
            var cate = _db.categories.SingleOrDefault(t => t.Id == Id);
            return View(cate);
        }

        [HttpPost]
        public ActionResult Edit(Category c)
        {
            var catedb = _db.categories.SingleOrDefault(t => t.Id == c.Id);
            catedb.Name = c.Name;
            catedb.Description = c.Description;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int Id)
        {
            var Catedb = _db.categories.SingleOrDefault(t => t.Id == Id);
            _db.categories.Remove(Catedb);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}