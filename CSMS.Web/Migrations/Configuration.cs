namespace CSMS.Web.Migrations
{
    using CSMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security.DataProtection;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web;

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(DataContext context)
        {
            foreach (string role in new string[] { "Admin", "Customer" }) 
            {
                if (context.Roles.Any(role_ => role_.Name == role)) continue;
                context.Roles.Add(new IdentityRole 
                {
                    Name = role
                });
            }

            context.SaveChanges();
            if (!context.Users.Any(user => user.UserName == "Admin"))
            {
                using (ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context)))
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        Name = "John",
                        Surname = "Doe",
                        Email = "admin@example.com",
                        Address = "123 Main St, Anytown, USA",
                        PhoneNumber = "555-555-1212",
                        UserName = "admin@example.com",
                        SecurityStamp = Guid.NewGuid().ToString("D"),
                        LockoutEnabled = false,
                        EmailConfirmed = true
                    };

                    user.PasswordHash = userManager.PasswordHasher.HashPassword("Admin@123456"); ;
                    context.Users.Add(user);
                    user.Roles.Add(new IdentityUserRole()
                    {
                        RoleId = (from role in context.Roles where role.Name == "Admin" select role.Id).SingleOrDefault(),
                        UserId = user.Id
                    });
                }

                context.SaveChanges();
            }
        }
    }
}
