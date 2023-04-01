using CSMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CSMS.Web.Controllers
{


    [Authorize(Roles = "Customer")]
    public class CustomerPanelController : Controller
    {
        public CustomerPanelController()
        {
        }

        public CustomerPanelController(ApplicationUserManager userManager)
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
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var uinfo = UserManager.Users.FirstOrDefault(x => x.Id == currentUserId);

            if (uinfo != null)
                return View(uinfo);
            return View("Not Found");
        }
    }
}