using System.Linq;
using System.Web.Mvc;
using CSMS.Web.Abstract;
using CSMS.Models.Service;
using CSMS.Models;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace CSMS.Web.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly Repository<Car> car = new Repository<Car>();
        public CarController()
        {
        }

        public CarController(ApplicationUserManager userManager)
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
            return View(car.Get(x => x.ApplicationUserId == currentUserId).OrderByDescending(x => x.CarId).Take(20).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Car p)
        {
            var currentUserId = User.Identity.GetUserId();
            p.ApplicationUserId = currentUserId;
            car.Insert(p);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var carID = car.GetById(id);
            return View(carID);
        }
        [HttpPost]
        public ActionResult Edit(Car p)
        {
            var edit = car.GetById(p.CarId);
            edit.Brand = p.Brand;
            edit.Model = p.Model;
            edit.Year = p.Year;
            edit.Type = p.Type;
            edit.Plate = p.Plate;
            car.Update(edit);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var delete = car.GetById(id);
            car.Delete(delete);
            return RedirectToAction("Index");

        }

    }
}