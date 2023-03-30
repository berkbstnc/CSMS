using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using CSMS.Models;

namespace CSMS.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CSMS.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            foreach (string role in new string[] { "Admin", "Customer" }) {
                var isExist = !string.IsNullOrWhiteSpace((from role_ in context.Roles where role_.Name == role select role_.Name).SingleOrDefault());
                if (isExist) continue;

                context.Roles.AddOrUpdate(new IdentityRole {
                    Name = role
                });
            }

            context.SaveChanges();
        }
    }
}
