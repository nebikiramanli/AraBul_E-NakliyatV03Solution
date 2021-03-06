using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AraBulNakliyat.BusinessLayer;
using AraBulNakliyat.BusinessLayer.Abstract;
using AraBulNakliyat.Common.Helpers;
using AraBulNakliyat.DataAccessLayer.EntityFrameWork;
using AraBulNakliyat.Entities;
using AraBulNakliyat.Entities.Messages;
using AraBulNakliyat.Entities.ValueObjects;

namespace AraBulNakliyat.BusinessLayer
{
    public class AraBulUserManager:ManagerBase<AraBulUser>
    {
        private  Repository<AraBulUser> repo_user = new Repository<AraBulUser>();
        public BusinessLayerResult<AraBulUser> RegisterUser(RegisterViewModel data)
        {
            AraBulUser user =  repo_user.Find(x => x.UserName == data.UserName || x.Email == data.EMail);
          BusinessLayerResult<AraBulUser> layerResult =  new BusinessLayerResult<AraBulUser>();
          if (user != null)
          {
              if (user.UserName == data.UserName)
              {
                  layerResult.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists,"Kullanıcı adı kayıtlı");
              }

              if (user.Email == data.EMail)
              {
                  layerResult.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-Posta Adresi Kayıtlı");
              }
          }
          else
          {
              int dbResult =repo_user.Insert(new AraBulUser()
              {
                  UserName = data.UserName,
                  Email = data.EMail,
                  ProfileImageFilename = "user_boy.png",
                  Password = data.Password,
                  ActivateGuid = Guid.NewGuid(),
                  IsActive = false, 
                  IsAdmin = false
              });
              if (dbResult > 0)
              {
                 layerResult.Result= repo_user.Find(x => x.Email == data.EMail && x.UserName == data.UserName);
                    // : Akativasyon Mail İşlemi 
                     //  layerResult.Result.ActivateGuid
                     string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                     string activeUri = $"{siteUri}Home/UserActive/{layerResult.Result.ActivateGuid}";
                     string body =
                         $"{layerResult.Result.UserName}; <br> Hesabınızı aktiflştirmek için <a href= '{activeUri}' target ='_blank'> tıklayanız </a>";
                     MailHelper.SendMail(body,layerResult.Result.Email,"AraBulNakliyat Hesap AktifLeştirme");
              }
          }

          return layerResult;
         }

        public BusinessLayerResult<AraBulUser> LoginUser(LoginViewModel data)
        {
            
            BusinessLayerResult<AraBulUser> res = new BusinessLayerResult<AraBulUser>();
            res.Result = repo_user.Find(x => x.UserName == data.UserName && x.Password == data.Password);
            
            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                {
                    res.AddError(Entities.Messages.ErrorMessageCode.UserIsNotActive, "Kullanıcı Aktif Değil");
                   res.AddError(Entities.Messages.ErrorMessageCode.CheckYourEmail, "Lütfen e-posta Adresinizi Kontrol Ediniz");
                }
               
            }
            else
            {
              res.AddError(Entities.Messages.ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı Adı yada Şifre Uyuşmuyor");
            }

            return res;
        }

        public BusinessLayerResult<AraBulUser> ActiveUser(Guid id)
        {
            BusinessLayerResult<AraBulUser> res = new BusinessLayerResult<AraBulUser>();
            res.Result = repo_user.Find(x => x.ActivateGuid== id);

            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlreadyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }

                res.Result.IsActive = true;
                repo_user.Update(res.Result);

            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExists,"Aktifleştirilecek Kullanıcı Bulunamadı");
            }

            return res;
        }

        public BusinessLayerResult<AraBulUser> GetById(int id)
        {
            BusinessLayerResult<AraBulUser>  res= new BusinessLayerResult<AraBulUser>();
            res.Result = repo_user.Find(x => x.Id == id);
            if (res.Result== null)
            {
                res.AddError(ErrorMessageCode.UserNotFound,"Kullanıcı Bulunamadı");
            }

            return res;
          
        }

        public BusinessLayerResult<AraBulUser> UpdateProfile(AraBulUser data)
        {
            AraBulUser db_user = Find(x => x.Id != data.Id && (x.UserName == data.UserName || x.Email == data.Email));
            BusinessLayerResult<AraBulUser> res = new BusinessLayerResult<AraBulUser>();
            res.Result = data;
            if (db_user != null && db_user.Id != null)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta Adresi Kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;



            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdated, "Kullanıcı Güncellenmedi ");
            }

            return res;
        }
        public BusinessLayerResult<AraBulUser> RemoveUserById(int id)
        {
            BusinessLayerResult<AraBulUser> res = new BusinessLayerResult<AraBulUser>();
            AraBulUser user = Find(x => x.Id == id);
            if (user != null)
            {
                if (Delete(user) == 0)
                {
                    res.AddError(ErrorMessageCode.UserCouldNotRemove, "Kullanıcı Silinemedi");
                }

            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı Bulunamadı");
            }

            return res;
        }
        // Method Hiding arastır

        public new BusinessLayerResult<AraBulUser> Insert(AraBulUser data)
        {
            AraBulUser user = Find(x => x.UserName == data.UserName || x.Email == data.Email);
            BusinessLayerResult<AraBulUser> layerResult = new BusinessLayerResult<AraBulUser>();
            layerResult.Result = data;
            if (user != null)
            {
                if (user.UserName == data.UserName)
                {
                    layerResult.AddError(Entities.Messages.ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı adı kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    layerResult.AddError(Entities.Messages.ErrorMessageCode.EmailAlreadyExists, "E-Posta Adresi Kayıtlı");
                }
            }
            else
            {
                layerResult.Result.ProfileImageFilename = "user_boy.png";
                layerResult.Result.ActivateGuid = Guid.NewGuid();
                if (base.Insert(layerResult.Result) == 0)
                {
                    layerResult.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenemedi");
                }

            }

            return layerResult;
        }
        public new BusinessLayerResult<AraBulUser> Update(AraBulUser data)
        {
            AraBulUser db_user = Find(x => x.Id != data.Id && (x.UserName == data.UserName || x.Email == data.Email));
            BusinessLayerResult<AraBulUser> res = new BusinessLayerResult<AraBulUser>();
            if (db_user != null && db_user.Id != null)
            {
                if (db_user.UserName == data.UserName)
                {
                    res.AddError(ErrorMessageCode.UsernameAlreadyExists, "Kullanıcı Adı Kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExists, "E-posta Adresi Kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.UserName = data.UserName;

            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profiliniz Güncellenmedi ");
            }

            return res;
        }
    }
}
