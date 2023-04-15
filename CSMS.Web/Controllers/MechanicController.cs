using CSMS.Models;
using CSMS.Models.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using CSMS.Web.Abstract;

namespace CSMS.Web.Controllers
{
    [Authorize]
    public class MechanicController : Controller
    {
        private ApplicationUserManager _userManager;
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

        private ApplicationRoleManager _roleManager;
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

        public MechanicController()
        {
        }

        public MechanicController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index()
        {
            // TODO: Burasinin duzeltilmesi gerekiyor.
            ApplicationUser[] mechanicUsers = (from user in UserManager.Users select user)
                .ToArray()
                .Where(user => UserManager.IsInRole(user.Id, "Mechanic"))
                .Take(10)
                .ToArray();
            ViewBag.MechanicUsers = mechanicUsers;
            return View();
        }
    }
}
