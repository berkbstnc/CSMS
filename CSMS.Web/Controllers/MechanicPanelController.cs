using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CSMS.Web.Controllers
{


    [Authorize(Roles = "Mechanic")]
    public class MechanicPanelController : Controller
    {
        public MechanicPanelController() { }

        public MechanicPanelController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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
        [HttpGet]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            ApplicationUser user = UserManager.Users.FirstOrDefault(x => x.Id == userId);
            return View(user);
        }
    }
}