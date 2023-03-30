using System.Linq;
using System.Web.Mvc;
using CSMS.Models.Service;
using CSMS.Web.Abstract;

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