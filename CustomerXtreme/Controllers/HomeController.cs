using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CustomerXtreme.Controllers
{
    public class HomeController : Controller
    {
        private customerEntities db = new customerEntities();

        // GET: Customers
        public ActionResult Index(string search, string Searchby)
        {
            if (Session["Username"] != null)
            {
                if (Searchby == "Code")
                {
                    var model = db.Customers.Where(a => a.code == search || search == null).ToList();
                    return View(model);

                }
                else
                {
                    //string r = search.ToString();
                    var model = db.Customers.Where(a => a.name.StartsWith(search) || search == null).ToList();
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public JsonResult IsCodeValid(string code)
        {
            return Json(!db.Customers.Any(x => x.code == code), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer objCustomer)
        {
            if (ModelState.IsValid)
            {
                var obj = db.Customers.Where(a => a.name.Equals(objCustomer.name) && a.password.Equals(objCustomer.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["Username"] = obj.name.ToString();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or password is wrong";
                    // return RedirectToAction("Login");
                    return View();
                }
            }

            return View(objCustomer);
        }

        // GET: Customers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "code,name,password,dob,email,phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "code,name,password,dob,email,phone")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}