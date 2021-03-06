using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AraBulNakliyat.ViewsModels
{
    public class OkViewModel: NotifyViewModelBase<string>
    {
        public OkViewModel()
        {
            Title = "İşlem Başarılı";
        }
    }
}