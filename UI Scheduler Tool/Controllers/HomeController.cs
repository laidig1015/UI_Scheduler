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
            return Redirect("https://sts.uiowa.edu/adfs/ls/?wa=wsignin1.0&wtrealm=urn:federation:MicrosoftOnline&wctx=wa%3Dwsignin1.0%26rpsnv%3D3%26ct%3D1401813569%26rver%3D6.1.6206.0%26wp%3DMBI_SSL%26wreply%3Dhttps:%252F%252Fpod51041.outlook.com%252Fowa%252F%26id%3D260563%26whr%3Duiowa.edu%26CBCXT%3Dout%26vv%3D2010");
        }
    }
}
