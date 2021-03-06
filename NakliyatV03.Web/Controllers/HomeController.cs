using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using AraBulNakliyat.BusinessLayer;
using AraBulNakliyat.Entities;
using AraBulNakliyat.Entities.Messages;
using AraBulNakliyat.Entities.ValueObjects;
using AraBulNakliyat.ViewsModels;
using NakliyatV03.Web.Filters;
using NakliyatV03.Web.Models;


namespace AraBulNakliyat.Controllers   
{
    [Exeption]
    public class HomeController : Controller
    {
        private NoticeManager noticeManager = new NoticeManager();
        private CityManager cityManager = new CityManager();
        private AraBulUserManager araBulUserManager = new AraBulUserManager();
        // GET: Home
        public ActionResult Index()
        {
            

            // Category ile tempdata 
          // Category contorlunden gelen tempdatayı ısleyıp view ve model olarak gonderdıgımız yer 
            //if (TempData["mm"] !=null)
            //{
            //    return View(TempData["mm"] as List<Note>);
            //}
            return View(noticeManager.ListQueryable().Where(x=>x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
            //return View(noteManager.GetAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult ByCity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            City city = cityManager.Find(x => x.Id == id.Value);
            if (city == null)
            { 
                return HttpNotFound();
            }


            return View("Index", city.Notices.Where(x=>x.IsDraft ==false).OrderByDescending(x =>x.ModifiedOn ).ToList());
        }
        public ActionResult MostLiked()
        {
            
            return View("Index", noticeManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        }

        public ActionResult About()
        {
            return View();
        }
        [AuthLogin]
        public ActionResult ShowProfile()
        {
            BusinessLayerResult<AraBulUser> res = araBulUserManager.GetById(CurrentSession.User.Id);

            if (res.Errors.Count >0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                
                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [AuthLogin]
        public ActionResult EditProfile()
        {
            BusinessLayerResult<AraBulUser> res = araBulUserManager.GetById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };


                return View("Error", errorNotifyObj);
            }

            return View(res.Result);
        }
        [AuthLogin]
        [HttpPost]
        public ActionResult EditProfile(AraBulUser model,HttpPostedFileBase ProfileImage)
        {
            //ModifiedUserName i .kontrol etmesini kaldırdık
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (ProfileImage != null && (
                    ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string fileName = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{fileName}"));
                    model.ProfileImageFilename = fileName;
                }

                BusinessLayerResult<AraBulUser> res = araBulUserManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Geçersiz İşlem",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    //TempData["errors"] = res.Errors;
                    return View("Error", errorNotifyObj);
                }
                //Profil Güncellendiği İçin Session da Güncellenir
                Session["login"] = res.Result;

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }
        [AuthLogin]
        public ActionResult DeleteProfile()
        {
            BusinessLayerResult<AraBulUser> res = araBulUserManager.RemoveUserById(CurrentSession.User.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile"
                };
                return View("Error", errorNotifyObj);
            }
            Session.Clear();

            return RedirectToAction("Index");
        }
      
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
           if (ModelState.IsValid) {
              
                BusinessLayerResult<AraBulUser> res = araBulUserManager.LoginUser(model);
                  if (res.Errors.Count > 0)
                  {
                      // HAktif Olmayan kullanıcıların girişte Email e yonlediren view bag 
                      if (res.Errors.Find(x=>x.Code== ErrorMessageCode.UserIsNotActive) !=null)
                      {
                          // buraya gmail in url si gelebilir
                          ViewBag.Setlink = "http://Home/active";
                      }
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                     return View(model);
                  }

                CurrentSession.Set<AraBulUser>("login", res.Result);   //Session İle Bilgi Saklama İşlemi
                return  RedirectToAction("Index"); }

           return View(model);
        }
        public ActionResult Register()
        {
            
            return View();
         }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)

        {
            if (ModelState.IsValid)
            {
               
                BusinessLayerResult<AraBulUser> res = araBulUserManager.RegisterUser(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/Login",

                };
                notifyObj.Items.Add("Lütfen e-posta Adresinize gönderdiğimiz aktivasyon linkine tıklayarak hesabınızı aktive ediniz.Hesabınızı aktive etmeden beğenme ve yorum işlemi yapılamaz.");

                return View("Ok",notifyObj);
            }

            return View(model);
        }

        public ActionResult UserActive(Guid id)
        {
            AraBulUserManager eum =new AraBulUserManager();
              BusinessLayerResult<AraBulUser> res = eum.ActiveUser(id);
              if (res.Errors.Count>0)
              {
                  ErrorViewModel errorNotifyObj = new ErrorViewModel()
                  {
                      Title = "Geçersiz İşlem",
                      Items = res.Errors
                  };

                  //TempData["errors"] = res.Errors;
                  return View( "Error" , errorNotifyObj);
              }

              OkViewModel okNotifyObj = new OkViewModel()
              {
                  Title = "Hesap Aktifleştirildi",
                  RedirectingUrl = "/Home/Login",
              };
              okNotifyObj.Items.Add("Hesabınız Aktifleştirildi. Artık Not Paylaşabilir ve Beğenme Yapabilirsiniz");
              return View("Ok",okNotifyObj );
        }

       
        public ActionResult Logout()
        {
            CurrentSession.Clear();
            //Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}