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
    [Exeption]
    public class NoticeController : Controller
    {
        private NoticeManager _noticeManager = new NoticeManager();
        private CityManager _cityManager = new CityManager();
        private LikedManager _likedManager = new LikedManager();

        // GET: Notice
        [AuthLogin]
        public ActionResult Index()
        {
            var notices = _noticeManager.ListQueryable().
                Include("City").Include("Owner")
                .Where(x => x.Owner.Id == CurrentSession.User.Id)
                .OrderByDescending(x => x.ModifiedOn);
            return View(notices.ToList());
        }
        // GET: MostLiked
        public ActionResult MyLikedNotice()
        {
            var notice = _likedManager.ListQueryable().Include("LikedUser").
                Include("Notice").
                Where(x => x.LikedUser.Id == CurrentSession.User.Id).
                Select(x => x.Notice).Include("City").Include("Owner")
                .OrderByDescending(x => x.ModifiedOn);



            return View("Index", notice.ToList());

        }

        // GET: Notice/Details/5
        [AuthLogin]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Notice notice = _noticeManager.Find(x => x.Id == id.Value);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // GET: Notice/Create
        [AuthLogin]
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(_cityManager.List(), "Id", "Title");
            return View();
        }

        // POST: Notice/Create
        [AuthLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Notice notice)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                
                notice.Owner = CurrentSession.User;
                _noticeManager.Insert(notice);
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(_cityManager.List(), "Id", "Title", notice.CityId);
            return View(notice);
        }

        // GET: Notice/Edit/5
        [AuthLogin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Notice notice = _noticeManager.Find(x => x.Id == id.Value);
            if (notice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(_cityManager.List(), "Id", "Title", notice.CityId);
            return View(notice);
        }

        // POST: Notice/Edit/5
        [AuthLogin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Notice notice)
        {
            ModelState.Remove("CreateOn");
            ModelState.Remove("Modifiedon");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                Notice db_notice = _noticeManager.Find(x => x.Id == notice.Id);
                db_notice.IsDraft = notice.IsDraft;
                db_notice.CityId = notice.CityId;
                db_notice.Text = notice.Text;
                db_notice.Title = notice.Title;
                _noticeManager.Update(db_notice);

                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(_cityManager.List(), "Id", "Title", notice.CityId);
            return View(notice);
        }

        // GET: Notice/Delete/5
        [AuthLogin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Notice notice = _noticeManager.Find(x => x.Id == id.Value);
            if (notice == null)
            {
                return HttpNotFound();
            }
            return View(notice);
        }

        // POST: Notice/Delete/5
        [AuthLogin]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Notice notice = _noticeManager.Find(x => x.Id == id);
            _noticeManager.Delete(notice);
           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            if (CurrentSession.User != null)
            {
                List<int> likedNoticeIds = _likedManager.List(
                    x => x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(x.Notice.Id)).Select(
                    x => x.Notice.Id).ToList();

                return Json(new { result = likedNoticeIds });
            }
            else
            {
                return Json(new { result = new List<int>() });
            }
        }

        [HttpPost]
        public ActionResult SetLikeState(int noticeid, bool liked)
        {
            int res = 0;

            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            Liked like =
                _likedManager.Find(x => x.Notice.Id == noticeid && x.LikedUser.Id == CurrentSession.User.Id);

            Notice notice = _noticeManager.Find(x => x.Id == noticeid);

            if (like != null && liked == false)
            {
                res = _likedManager.Delete(like);
            }
            else if (like == null && liked == true)
            {
                res = _likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Notice = notice
                });
            }

            if (res > 0)
            {
                if (liked)
                {
                    notice.LikeCount++;
                }
                else
                {
                    notice.LikeCount--;
                }

                res = _noticeManager.Update(notice);

                return Json(new { hasError = false, errorMessage = string.Empty, result = notice.LikeCount });
            }

            return Json(new { hasError = true, errorMessage = "Beğenme işlemi gerçekleştirilemedi.", result = notice.LikeCount });
        }


        public ActionResult GetNoticeText(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Notice notice = _noticeManager.Find(x => x.Id == id);

            if (notice == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialNoticeText", notice);
        }
    }
}
