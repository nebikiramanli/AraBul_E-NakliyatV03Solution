using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NakliyatV03.Web.Models;

namespace NakliyatV03.Web.Filters
{
    public class AuthLogin : FilterAttribute,IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User ==null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
            }
        }
    }
}