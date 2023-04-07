namespace CSMS.Web.Migrations
{
    using CSMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<CSMS.Web.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(CSMS.Web.DataContext context)
        {
            foreach (string role in new string[] { "Admin", "Customer" }) {
                if (context.Roles.Any(role_ => role_.Name == role)) continue;
                context.Roles.Add(new IdentityRole {
                    Name = role
                });
            }

            context.SaveChanges();
            if (!context.Users.Any(user => user.UserName == "Admin"))
            {
                ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                ApplicationUser adminApplicationUser = new ApplicationUser()
                {
                    Name = "Admin",
                    Surname = "Admin",
                    Email = "admin@example.com",
                    Address = "",
                    PhoneNumber = "55",
                    UserName = "Admin"
                };
                userManager.Create(adminApplicationUser, "Admin@123456");
                adminApplicationUser.Roles.Add(new IdentityUserRole() {
                    RoleId = (from role in context.Roles where role.Name == "Admin" select role.Id).SingleOrDefault(),
                    UserId = adminApplicationUser.Id
                });
                context.SaveChanges();
            }
        }
    }
}
