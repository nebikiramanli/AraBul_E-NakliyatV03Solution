using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AraBulNakliyat.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        //Text
        [Required(ErrorMessage = "{0} alanı Gereklidir."),
         StringLength(300,ErrorMessage = "{0} Alanı max. {1} Kadar Olmalıdır"),
        DisplayName("Yorum")]
        public string Text { get; set; }

        public virtual Notice Notice { get; set; }

        public virtual AraBulUser Owner { get; set; }
    }
}