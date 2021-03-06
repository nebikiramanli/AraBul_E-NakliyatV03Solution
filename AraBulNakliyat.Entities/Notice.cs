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
    [Table("Notices")]
    public class Notice : MyEntityBase
    {
        [DisplayName("İlan Başlığı"),
            Required(ErrorMessage = "{0} Alanı Boş Bırakmayınız"),
            StringLength(100, ErrorMessage = "{0} Alanı max. {1} Kadar Olmalıdır")]
        public string Title { get; set; }


        [DisplayName("İlan Notu"),
            Required(ErrorMessage = "{0} Alanı Boş Bırakmayınız"),
         StringLength(2000,ErrorMessage = "{0} Alanı max. {1} Kadar Olmalıdır")]
        public string Text { get; set; }

        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }

        //Otomatik olarak Required olarak Tanımlanıyor
        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }

        //Otomatik olarak Required olarak Tanımlanıyor
        [DisplayName("Şehir")]
        public int CityId { get; set; }

        public virtual AraBulUser Owner { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual City City { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Notice()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }
    }
}