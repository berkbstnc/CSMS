using System.Linq;
using System.Web.Mvc;
using CSMS.Models.Service;
using CSMS.Web.Abstract;
using CSMS.Web.Models.Service;

namespace CSMS.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly Repository<Car> Nuser = new Repository<Car>();
        private readonly Repository<FaultRecord> record = new Repository<FaultRecord>();
        public ActionResult Index(string search)
        {
            if (search == "" || search == null)
            {
                var user = Nuser.Get();
                return View(user);
            }
            var searchItem = Nuser.Get(x => x.Name.Contains(search) || x.Model.Contains(search) || x.Plate.Contains(search)).ToList(); //Contains-StartsWith kullanılabilir.
            return View(searchItem);
        }

        public ActionResult OrderCreate(int customerid)
        {
            ViewBag.CustomerId = customerid;
            return View();
        }

        public ActionResult Create(FaultRecord faultRecord)
        {
            record.Insert(faultRecord);
            return RedirectToAction("OpenedOrders");
        }

        public ActionResult OpenedOrders()
        {
            return View(record.Get(x => x.Status == false).ToList());
        }
    }

}