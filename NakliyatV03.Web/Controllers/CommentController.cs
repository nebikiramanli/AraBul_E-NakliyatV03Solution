using System;
using System.Collections.Generic;
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
    public class CommentController : Controller
    {
        private NoticeManager noticeManager = new NoticeManager();
        private CommentManager commentManager = new CommentManager();

        public ActionResult ShowNoticeComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //Note note = noteManager.Find(x => x.Id == id);
            Notice notice = noticeManager.ListQueryable().Include("Comments").FirstOrDefault(x => x.Id == id);

            if (notice == null)
            {
                return HttpNotFound();
            }

            return PartialView("_PartialComments", notice.Comments);
        }

        [AuthLogin]
        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            comment.Text = text;

            if (commentManager.Update(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [AuthLogin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Comment comment = commentManager.Find(x => x.Id == id);

            if (comment == null)
            {
                return new HttpNotFoundResult();
            }

            if (commentManager.Delete(comment) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        [AuthLogin]
        [HttpPost]
        public ActionResult Create(Comment comment, int? noticeid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                if (noticeid == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                Notice notice = noticeManager.Find(x => x.Id == noticeid);

                if (notice == null)
                {
                    return new HttpNotFoundResult();
                }

                comment.Notice = notice;
                comment.Owner = CurrentSession.User;

                if (commentManager.Insert(comment) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }
    }
}