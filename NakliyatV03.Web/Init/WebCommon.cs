using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AraBulNakliyat.Common;
using AraBulNakliyat.Entities;
using NakliyatV03.Web.Models;

namespace AraBulNakliyat.Init
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUserName()
        {
            AraBulUser user = CurrentSession.User;
            if (user != null)
            {
                return user.UserName;
            }
            else
            {
                return "system";
            }
        }
    }
}