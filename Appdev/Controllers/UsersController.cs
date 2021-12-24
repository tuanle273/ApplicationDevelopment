using Appdev.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static Appdev.Controllers.AccountController;

namespace Appdev.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private ApplicationDbContext _db;
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        public UsersController()
        {
            _db = new ApplicationDbContext();
        }

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [Authorize(Roles = "Admin,Staff")]
        // GET: Users
        public async Task<ActionResult> Index()
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

            var userList = _db.Users.ToList();
            List<Appdev.Models.User> userFinal = new List<User>();
            foreach (var user in userList)
            {
                var roleTemp = await UserManager.GetRolesAsync(user.Id);
                Appdev.Models.User userbase = new User();
                userbase.Role = roleTemp.First();
                userbase.Email = user.Email;
                userbase.FullName = user.FullName;
                userbase.Id = user.Id;
                userFinal.Add(userbase);
            }


            if (User.IsInRole("Admin"))
            {
                var userOfAdminRole = userFinal.Where(u => u.Role != "Trainee" && u.Id != userIdCurrentLogin);
                return View(userOfAdminRole);
            }

            var userOfStaffRole = userFinal.Where(u => u.Role == "Trainer" || u.Role == "Trainee").Where(u => u.Id != userIdCurrentLogin);
            return View(userOfStaffRole);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var roleTemp = await UserManager.GetRolesAsync(id);
            if (roleTemp.First() == "Admin")
            {
                return RedirectToAction("EditAdmin", new { id = id });
            }
            if (roleTemp.First() == "Staff")
            {
                return RedirectToAction("EditStaff", new { id = id });
            }
            if (roleTemp.First() == "Trainer")
            {
                return RedirectToAction("EditTrainer", new { id = id });
            }

            return RedirectToAction("EditTrainee", new { id = id });
        }

        [Authorize(Roles = "Admin,Staff")]
        public ActionResult EditAdmin(string id)
        {
            var user = _db.Admins.Find(id);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin(Admin user)
        {
            var userFromDb = _db.Admins.Find(user.Id);
            if (userFromDb == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.FullName = user.FullName;
                userFromDb.Age = user.Age;
                userFromDb.Address = user.Address;
                _db.Entry(userFromDb).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize(Roles = "Admin,Staff")]
        public ActionResult EditStaff(string id)
        {
            var user = _db.Staffs.Find(id);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        [ValidateAntiForgeryToken]
        public ActionResult EditStaff(Staff user)
        {
            var userFromDb = _db.Staffs.Find(user.Id);
            if (userFromDb == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.FullName = user.FullName;
                userFromDb.Age = user.Age;
                userFromDb.Address = user.Address;
                _db.Entry(userFromDb).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize(Roles = "Admin,Trainer")]
        public ActionResult EditTrainer(string id)
        {
            if (id == null)
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
                id = userIdCurrentLogin;
            }

            var user = _db.Trainers.Find(id);
            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Trainer")]
        [ValidateAntiForgeryToken]
        public ActionResult EditTrainer(Trainer user)
        {
            var userFromDb = _db.Trainers.Find(user.Id);
            if (userFromDb == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.FullName = user.FullName;
                userFromDb.Age = user.Age;
                userFromDb.Address = user.Address;
                userFromDb.Speciality = user.Speciality;
                _db.Entry(userFromDb).State = EntityState.Modified;
                _db.SaveChanges();

                if (User.IsInRole("Trainer"))
                {
                    ViewData["Message"] = "Success: Update user info";
                    return View(user);
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult EditTrainee(string id)
        {
            if (id == null)
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
                id = userIdCurrentLogin;
            }

            var user = _db.Trainees.Find(id);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Staff,Trainee")]
        public ActionResult EditTrainee(Trainee user)
        {
            var userFromDb = _db.Trainees.Find(user.Id);
            if (userFromDb == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                userFromDb.PhoneNumber = user.PhoneNumber;
                userFromDb.FullName = user.FullName;
                userFromDb.Age = user.Age;
                userFromDb.Address = user.Address;
                userFromDb.DateOfBirth = user.DateOfBirth;
                userFromDb.Education = user.Education;
                _db.Entry(userFromDb).State = EntityState.Modified;
                _db.SaveChanges();

                if (User.IsInRole("Trainee"))
                {
                    ViewData["Message"] = "Success: Update user info";
                    return View(user);
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Staff")]
        public ActionResult Delete(string id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            _db.Users.Remove(user);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}