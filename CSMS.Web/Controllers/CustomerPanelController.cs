using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.BuilderProperties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CSMS.Web.Controllers
{


    [Authorize(Roles = "Customer")]
    public class CustomerPanelController : Controller
    {
        private readonly Repository<Car> car = new Repository<Car>();
        private readonly Repository<ApplicationUser> apuser = new Repository<ApplicationUser>();
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
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,Name,Surname,Address,PhoneNumber")] EditUserViewModel editUser)
        {

         var user = await UserManager.FindByIdAsync(editUser.Id);
         user.Email = editUser.Email;
         user.Name = editUser.Name;
         user.Surname = editUser.Surname;
         user.Address = editUser.Address;
         user.PhoneNumber = editUser.PhoneNumber;
         UserManager.Update(user);
         return RedirectToAction("Index");
        }

    }
}