using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AraBulNakliyat.Entities
{
    [Table("AraBulUsers")]
    public class AraBulUser : MyEntityBase
    { 
        // Name
        [DisplayName("İsim"),
         StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        //Surname
        [StringLength(25 ,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır."),
        DisplayName("Soyad")]
        public string Surname { get; set; }

        //Username
        [Required(ErrorMessage = "{0} Alanı Gereklidir"), 
         StringLength(25,ErrorMessage = "{0} alanı max. {1}karakter olmalıdır."),
        DisplayName("Kullanıcı Adı")]
        public string UserName { get; set; }

        //Email
        [DisplayName("E-Posta"),
            Required(ErrorMessage = "{0} alanı Gereklidir"), 
         StringLength(70,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        //Password
        [DisplayName("Şifre"),
            Required(ErrorMessage = "{0} Alanı Gereklidir"),
         StringLength(70,ErrorMessage= "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }


        //profileİmage
        [StringLength(30),
        ScaffoldColumn(false)]
        public string ProfileImageFilename { get; set; }

        //IsAktif
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        //IsAdmin
        [DisplayName("Admin")]
        public bool IsAdmin { get; set; }

        
        //Guid
        [Required,
         ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual List<Notice> Notices { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }


    }
}