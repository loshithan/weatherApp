using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace weatherApp.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult NotFound()
        {
            return View();
        }

        
        public ActionResult UnAuthorized()
        {
            return View();
        }
        public ActionResult ForbiddenError()
        {
            return View();
        }
        public ActionResult BadRequest()
        {
            return View();
        }
        public ActionResult ServerError()
        {
            return View();
        }
        public ActionResult UnknownError()
        {
            return View();
        }
        public ActionResult SerializationError()
        {
            return View();
        }
        public ActionResult Timeout()
        {
            return View();
        }


    }
}