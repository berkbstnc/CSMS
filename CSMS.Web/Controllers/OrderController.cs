using CSMS.BusinessLayer.Abstract;
using CSMS.Entities.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSMS.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly Repository<Car> Nuser = new Repository<Car>();
        // GET: Order
        public ActionResult Index(string search)
        {
            if (search == "" || search == null)
            {
                var user = Nuser.Get();
                return View(user);
            }
            var searchItem = Nuser.Get(x => x.Name.Contains(search) || x.Model.Contains(search)).ToList(); //Contains-StartsWith kullanılabilir.
            return View(searchItem);
        }
    }

}