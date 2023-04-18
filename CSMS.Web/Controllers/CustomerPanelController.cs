using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
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
        private readonly Repository<Car> car = new Repository<Car>();
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
        [HttpGet]
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var uinfo = UserManager.Users.FirstOrDefault(x => x.Id == currentUserId);

            var list = car.Get(x => x.ApplicationUserId == currentUserId).Select(x => x.Plate).ToList();

            ViewBag.Data1 = list;


            if (uinfo != null)
                return View(uinfo);
            return View("Not Found");
        }
    }
}