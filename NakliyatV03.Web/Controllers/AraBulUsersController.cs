using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AraBulNakliyat.BusinessLayer;
using AraBulNakliyat.Entities;
using NakliyatV03.Web.Filters;
using NakliyatV03.Web.Models;

namespace NakliyatV03.Web.Controllers
{
    [AuthAdmin]
    [AuthLogin]
    [Exeption]
    public class AraBulUsersController : Controller
    {
       
        private  AraBulUserManager _araBulUserManager = new AraBulUserManager();
        // GET: AraBulUsers
        public ActionResult Index()
        {
            return View(_araBulUserManager.List());
        }

        // GET: AraBulUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AraBulUser araBulUser = _araBulUserManager.Find(x => x.Id == id.Value);
            if (araBulUser == null)
            {
                return HttpNotFound();
            }
            return View(araBulUser);
        }

        // GET: AraBulUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AraBulUsers/Create
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( AraBulUser araBulUser)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<AraBulUser> res = _araBulUserManager.Insert(araBulUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(araBulUser);
                }

                return RedirectToAction("Index");
            }

            return View(araBulUser);
        }

        // GET: AraBulUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AraBulUser araBulUser = _araBulUserManager.Find(x => x.Id == id.Value);
            if (araBulUser == null)
            {
                return HttpNotFound();
            }
            return View(araBulUser);
        }

        // POST: AraBulUsers/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( AraBulUser araBulUser)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<AraBulUser> res = _araBulUserManager.Update(araBulUser);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(araBulUser);
                }
                return RedirectToAction("Index");
            }
            return View(araBulUser);
        }

        // GET: AraBulUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AraBulUser araBulUser = _araBulUserManager.Find(x=>x.Id==id.Value);
            if (araBulUser == null)
            {
                return HttpNotFound();
            }
            return View(araBulUser);
        }

        // POST: AraBulUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AraBulUser araBulUser = _araBulUserManager.Find(x => x.Id == id);
            _araBulUserManager.Delete(araBulUser);
            return RedirectToAction("Index");
        }

        
    }
}
