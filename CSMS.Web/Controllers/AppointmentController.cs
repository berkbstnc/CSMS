using CSMS.Models;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace CSMS.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private Repository<Appointment> repository = new Repository<Appointment>();
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

        public AppointmentController()
        {
        }

        public AppointmentController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ActionResult Index()
        {
            ViewResult view = View();
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (UserManager.IsInRole(currentUser.Id, "Customer"))
            {
                ViewBag.Appointments = repository.List().Where(a => currentUser.Cars.Any(c => a.CarId == c.CarId)).ToArray();
                view.MasterName = "~/Views/Shared/_LayoutCustomer.cshtml";
            }
            else if (UserManager.IsInRole(currentUser.Id, "Mechanic"))
            {
                Appointment[] appointments = repository.List().Where(a => a.MechanicUserId == currentUser.Id && a.Status == 0).ToArray();
                ViewBag.Appointments = appointments;
                view.MasterName = "~/Views/Shared/_LayoutMechanic.cshtml";
            }
            return view;
        }

        public ActionResult Create(string id, string date)
        {
            DateTime appointmentDate = DateTime.Now.Date;   
            if (!string.IsNullOrEmpty(HttpContext.Request.Params["date"]))
            {
                date = HttpContext.Request.Params["date"];
            }

            if (!string.IsNullOrWhiteSpace(date))
            {
                if (!DateTime.TryParse(date, out appointmentDate))
                {
                    ViewBag.CustomError = "Invalid appointment date.";
                    return View();
                }

                if (appointmentDate.Date < DateTime.Now.Date)
                {
                    ViewBag.CustomError = "Appointment date cannot be less than today.";
                    return View();
                }
            }

            ViewBag.Date = appointmentDate;
            ApplicationUser user = UserManager.FindById(id);
            if (user == null)
            {
                ViewBag.CustomError = "There is no mechanic.";
                return View();
            }

            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id == currentUser.Id)
            {
                ViewBag.CustomError = "You cannot make appointment to yourself.";
                return View();
            }

            ViewBag.MechanicUser = user;
            var apps = user.Appointments;
            var apps2 = currentUser.Appointments;
            bool[] bookedHours = new bool[24];
            var existingAppointments = user.Appointments.Where(a => a.AppointmentDate.ToShortDateString() == appointmentDate.ToShortDateString()).OrderBy(a => a.AppointmentDate).ToArray();
            foreach (var appointment in existingAppointments)
            {
                int appointmentHour = appointment.AppointmentDate.Hour;
                bookedHours[appointmentHour] = true;
            }

            ViewBag.BookedHours = bookedHours;
            ViewBag.Cars = currentUser.Cars;
            return View();
        }

        [HttpPost]
        public ActionResult Create()
        {
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            string date = string.Format("{0} {1:00}:00:00", HttpContext.Request.Params["date"], HttpContext.Request.Params["hour"]);
            DateTime.TryParse(date, out DateTime appointmentDate);
            int carId = int.Parse(HttpContext.Request.Params["carId"]);
            string mechanicId = HttpContext.Request.Params["mechanicId"];
            string description = HttpContext.Request.Params["description"];
            Appointment appointment = new Appointment()
            {
                AppointmentDate = appointmentDate,
                CarId = carId,
                MechanicUserId = mechanicId,
                //Car = currentUser.Cars.Where(c => c.CarId == carId).SingleOrDefault(),
                //MechanicUser = UserManager.FindById(mechanicId),
                Description = description,
                Status = 0
            };
            
            repository.Insert(appointment);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CloseAppointment()
        {
            int appointmentId = int.Parse(HttpContext.Request.Params["appointmentId"]);
            Appointment appointment = repository.GetById(appointmentId);
            appointment.Status = 1;
            repository.Update(appointment);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult History()
        {
            ViewResult view = View();
            ApplicationUser currentUser = UserManager.FindById(User.Identity.GetUserId());
            if (UserManager.IsInRole(currentUser.Id, "Customer"))
            {
                ViewBag.Appointments = repository.List().Where(a => currentUser.Cars.Any(c => a.CarId == c.CarId && a.Status == 1)).ToArray();
                view.MasterName = "~/Views/Shared/_LayoutCustomer.cshtml";
            }
            else if (UserManager.IsInRole(currentUser.Id, "Mechanic"))
            {
                Appointment[] appointments = repository.List().Where(a => a.MechanicUserId == currentUser.Id && a.Status == 1).ToArray();
                ViewBag.Appointments = appointments;
                view.MasterName = "~/Views/Shared/_LayoutMechanic.cshtml";
            }
            return view;
        }
    }
}
