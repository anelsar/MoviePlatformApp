using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APP.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Index()
        {
            return View("Error");
        }
        // GET: Error
        public ActionResult MovieError()
        {
            return Content("Movie error", "text/plain");
        }

        public ActionResult RoleError()
        {
            return Content("Role error", "text/plain");
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}