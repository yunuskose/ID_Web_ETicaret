using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IDETicaret.Models;
using System.Web.Routing;

namespace IDETicaret.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private ETicaretEntities ocmde = new ETicaretEntities();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionPersister.account == null)
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            else
            {
                var roles = Roles.Split(new char[] { ',' });
                if (SessionPersister.account is Account)
                {
                    var account = (Account)SessionPersister.account;
                    if (account.IsAdmin)
                    {
                        if (roles.Count(r => r.Equals("Admin")) == 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "../Admin", action = "Login" }));
                        }
                    }
                    else // customer
                    {
                        if (roles.Count(r => r.Equals("Customer")) == 0)
                        {
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Customer", action = "Login" }));
                        }
                    }
                }
                if (SessionPersister.account is Vendor)
                {   
                    if (roles.Count(r => r.Equals("Vendor")) == 0)
                    {
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "../Vendor", action = "Login" }));
                    }
                }
                
            }
        }
    }
}