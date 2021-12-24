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
    [Authorize(Roles = "Admin,Staff")]
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
    }
}