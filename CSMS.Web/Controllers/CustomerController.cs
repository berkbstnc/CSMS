using CSMS.BusinessLayer.Abstract;
using CSMS.Entities.Service;
using CSMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CSMS.Web.Controllers.Service
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly Repository<Customer> customer = new Repository<Customer>();
        public ActionResult Index()
        {
            return View(customer.Get().OrderByDescending(x=> x.CustomerId).Take(20).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Customer customerP)
        {
            customer.Insert(customerP);
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int id)
        {
            var customerID = customer.GetById(id);
            return View(customerID);
        }
        [HttpPost]
        public ActionResult Edit(Customer customerP)
        {
            var edit = customer.GetById(customerP.CustomerId);
            edit.Name = customerP.Name;
            edit.Surname = customerP.Surname;
            edit.Address = customerP.Address;
            edit.Email = customerP.Email;
            edit.Phone = customerP.Phone;
            edit.Plate = customerP.Plate;
            customer.Update(edit);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var delete = customer.GetById(id);
            customer.Delete(delete);
            return RedirectToAction("Index");

        }

    }


}