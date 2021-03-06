using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AraBulNakliyat.Entities.ValueObjects
{
    public class LoginViewModel
    {
        [DisplayName("Kullanıcı Adı"), 
         Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), 
         StringLength(25, ErrorMessage = "{0} Max {1} Karakter Olmalı")]
        public string UserName { get; set; }
        [DisplayName("Şifre"),
         Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),
         DataType(DataType.Password), 
         StringLength(25, ErrorMessage = "{0} Max {1} Karakter Olmalı")]
        public string Password { get; set; }
    }
}