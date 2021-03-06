using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AraBulNakliyat.Entities.ValueObjects
{
   public class RegisterViewModel
    {
        [DisplayName("Kullanıcı Adı"),
         Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
         StringLength(25, ErrorMessage = "{0} Max {1} Karakter Olmalı")]
        public string UserName { get; set; }
        [DisplayName("E-Posta"),
        Required(ErrorMessage = "{0} Alanı Boş Ğeçilemez"),
        StringLength(70,ErrorMessage = "{0} max {1} karakter Olmalı"),
        EmailAddress(ErrorMessage = "{0} Alanı İçin Geçerli Bir E-posta Adresi Giriniz")]
        public string EMail { get; set; }
        [DisplayName("Şifre"),
         Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "{0} Max {1} Karakter Olmalı")]
        public string Password { get; set; }

        [DisplayName("Şifre Tekrar"),
         Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
         DataType(DataType.Password),
         StringLength(25, ErrorMessage = "{0} Max {1} Karakter Olmalı"),
        Compare("Password",ErrorMessage = "{0} ile  {1} uyuşmuyor")]
        public string RePassword { get; set; }
    }
}
