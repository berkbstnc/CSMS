using System.Linq;
using System.Web.Mvc;
using CSMS.Web.Abstract;
using CSMS.Models.Service;

namespace CSMS.Web.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private readonly Repository<Car> car = new Repository<Car>();
        public ActionResult Index()
        {
            return View(car.Get().OrderByDescending(x => x.CarId).Take(20).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Car p)
        {
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
            edit.Name = p.Name;
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