using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AraBulNakliyat.Entities
{
    [Table("Cities")]
    public class City : MyEntityBase
    {
        //Title
        [DisplayName("Şehir"),
         Required(ErrorMessage = "{0} alanı gereklidir."),
         StringLength(50, ErrorMessage = "{0} alanı max. {1} karakter içermeli.")]
        public string Title { get; set; }

        //Acıklama
        [StringLength(150,ErrorMessage = "{0} alanı max. {1} kadar olmalıdır."),
        DisplayName("Açıklama")]
        public string Description { get; set; }


        public virtual List<Notice> Notices { get; set; }

        public City()
        {
            Notices = new List<Notice>();
        }



    }
}