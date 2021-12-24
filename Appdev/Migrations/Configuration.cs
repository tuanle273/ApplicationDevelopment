namespace Appdev.Migrations
{
    using Appdev.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Appdev.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Appdev.Models.ApplicationDbContext context)
        {
            string[] roles = new string[] { "Admin", "Staff", "Trainer", "Trainee" };
            foreach (var role in roles)
            {
                if (!context.Roles.Any(r=> r.Name == role))
                {
                    context.Roles.Add(new IdentityRole(role));
                }
            }

            if(!context.Users.Any(u=>u.UserName == "Admin@gmail.com"))
            {
                var usermanager = new UserManager<Admin>(new UserStore<Admin>(context));
                var user = new Admin
                {
                    FullName = "NTBChung",
                    Email = "Admin@gmail.com",
                    UserName = "Admin@gmail.com",
                    PhoneNumber = "+111111111",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    PasswordHash = usermanager.PasswordHasher.HashPassword("Admin123@"),
                    LockoutEnabled = true,
                };
                usermanager.Create(user);
                usermanager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
