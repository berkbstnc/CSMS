using System;
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
    [Authorize]
    public class OrderController : Controller
    {
        private readonly Repository<Car> Nuser = new Repository<Car>();
        private readonly Repository<FaultRecord> record = new Repository<FaultRecord>();
        private readonly Repository<Period> Nperiod = new Repository<Period>();

        public OrderController(){}

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
                var user = Nuser.Get().OrderByDescending(x => x.CarId);
                return View(user);
            }
            var searchItem = Nuser.Get(x => x.ApplicationUser.Name.Contains(search) || x.ApplicationUser.Surname.Contains(search) || x.Plate.Contains(search)).ToList(); //Contains-StartsWith kullanılabilir.
            return View(searchItem);
        }

        public ActionResult OrderCreate(int carid)
        {
            ViewResult view = View();
            ViewBag.CarId = carid;
            if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                view.MasterName = "~/Views/Shared/_LayoutAdmin.cshtml";
            else if (UserManager.IsInRole(User.Identity.GetUserId(), "Mechanic"))
                view.MasterName = "~/Views/Shared/_LayoutMechanic.cshtml";
            return view;
        }

        public ActionResult Create(FaultRecord faultRecord)
        {
            record.Insert(faultRecord);
            if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                return RedirectToAction("OpenedOrders");

            return RedirectToAction("Index", "Appointment");
        }

        public ActionResult OpenedOrders()
        {
            return View(record.Get(x => x.Status == false).OrderByDescending(x => x.RecordId).ToList());
        }
        public ActionResult ClosedOrders()
        {
            return View(record.Get(x => x.Status == true).OrderByDescending(x => x.RecordId).ToList());
        }

        public ActionResult MakeOperation(int orderid)
        {
            var info = record.Get(x => x.RecordId == orderid, includeProperties: "CustomerCar").FirstOrDefault();
            ViewBag.Title = "Car Plate: " + info.CustomerCar.Plate;
            ViewBag.FaultId = orderid;
            ViewResult view = View(Nperiod.Get(x => x.FaultId == orderid).OrderByDescending(x => x.PeriodId).ToList());
            if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                view.MasterName = "~/Views/Shared/_LayoutAdmin.cshtml";
            else if (UserManager.IsInRole(User.Identity.GetUserId(), "Mechanic"))
                view.MasterName = "~/Views/Shared/_LayoutMechanic.cshtml";
            return view;
        }

        public ActionResult SaveOperation(Period period)
        {
            Nperiod.Insert(period);
            return RedirectToAction("MakeOperation", new {orderid = period.FaultId });
        }

        public ActionResult DeleteOperation(int id)
        {
            var delete = Nperiod.GetById(id);
            Nperiod.Delete(delete);
            return RedirectToAction("MakeOperation", new { orderid = delete.FaultId });
        }

        public ActionResult SaveClose(int FaultId, decimal TotalCost)
        {
            var FID = record.GetById(FaultId);
            FID.TotalCost = TotalCost;
            FID.Status = true;
            FID.FinishDate = DateTime.Now;
            record.Update(FID);
            if (UserManager.IsInRole(User.Identity.GetUserId(), "Admin"))
                return RedirectToAction("ClosedOrders");
            
            return RedirectToAction("Index", "Appointment");
        }

        public ActionResult SeeDetails(int orderid)
        {
            var info = record.Get(x => x.RecordId == orderid, includeProperties: "CustomerCar").FirstOrDefault();
            ViewBag.Title = "Fault Record ID: " + info.RecordId;
            ViewBag.FaultId = orderid;
            return View(Nperiod.Get(x => x.FaultId == orderid).OrderByDescending(x => x.PeriodId).ToList());
        }

    }

}