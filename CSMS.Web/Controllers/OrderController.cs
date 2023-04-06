using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
using CSMS.Web.Models.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace CSMS.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly Repository<Car> Nuser = new Repository<Car>();
        private readonly Repository<FaultRecord> record = new Repository<FaultRecord>();

        public OrderController()
        {
        }

        public OrderController(ApplicationUserManager userManager)
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
        public ActionResult Index(string search)
        {
            if (search == "" || search == null)
            {
                var user = Nuser.Get();
                return View(user);
            }
            var searchItem = Nuser.Get(x => x.ApplicationUser.Name.Contains(search) || x.ApplicationUser.Surname.Contains(search) || x.Plate.Contains(search)).ToList(); //Contains-StartsWith kullanılabilir.
            return View(searchItem);
        }

        public ActionResult OrderCreate(int carid)
        {
            ViewBag.CarId = carid;
            return View();
        }

        public ActionResult Create(FaultRecord faultRecord)
        {
            //var currentUserId = User.Identity.GetUserId();
            //faultRecord.CustomerCar.ApplicationUserId = currentUserId;
            record.Insert(faultRecord);
            return RedirectToAction("OpenedOrders");
        }

        public ActionResult OpenedOrders()
        {
            return View(record.Get(x => x.Status == false).ToList());
        }
    }

}