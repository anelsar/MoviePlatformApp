using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APP.Services
{
    public class HttpRequestValidationExceptionAttribute : FilterAttribute, IExceptionFilter
    {

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled && filterContext.Exception is HttpRequestValidationException)
            {
                filterContext.Result = new RedirectResult("https://localhost:44358/Error/RoleError");
                filterContext.ExceptionHandled = true;
            }
        }
    }

}