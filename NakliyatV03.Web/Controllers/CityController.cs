using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AraBulNakliyat.BusinessLayer;
using AraBulNakliyat.Entities;
using NakliyatV03.Web.Filters;

namespace NakliyatV03.Web.Controllers
{
    [AuthAdmin]
    [AuthLogin]
    [Exeption]
    public class CityController : Controller
    {
        private  CityManager _cityManager = new CityManager();
        // GET: City
        public ActionResult Index()
        {

            return View(_cityManager.List());
        }
        // GET: Category/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city= _cityManager.Find(x => x.Id == id.Value);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            
           
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(City city)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                _cityManager.Insert(city);
                //_categoryManager.Save();
                return RedirectToAction("Index");
            }

            return View(city);
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            City category = _cityManager.Find(x => x.Id == id.Value);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(City city)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                City cat = _cityManager.Find(x => x.Id == city.Id);
                cat.Title = city.Title;
                cat.Description = city.Description;
                _cityManager.Update(cat);
                return RedirectToAction("Index");
            }
            return View(city);
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            City city = _cityManager.Find(x => x.Id == id.Value);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city =_cityManager.Find(x => x.Id == id);
            _cityManager.Delete(city);

            return RedirectToAction("Index");
        }
    }
}