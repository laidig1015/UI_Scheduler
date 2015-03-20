using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI_Scheduler_Tool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult ICON()
        {
            return Redirect("http://icon.uiowa.edu");
        }

        public ActionResult ISIS()
        {
            return Redirect("http://isis.uiowa.edu");
        }

        public ActionResult Email()
        {
            return Redirect("http://email.uiowa.edu");
        }
    }
}
