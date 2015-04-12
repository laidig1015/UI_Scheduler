using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI_Scheduler_Tool.Models;
using UI_Scheduler_Tool.Models.Json;
using Newtonsoft.Json;

namespace UI_Scheduler_Tool.Controllers
{
    public class TrackController : Controller
    {
        public ActionResult Select()
        {
            return View();
        }

        public ActionResult EFA()
        {
            return View();
        }

        public ActionResult Builder()
        {
            return View();
        }

        public ActionResult GetNodes(string query)
        {
            using (var db = new DataContext())
            {
                List<Course> courses = db.Courses.ToList();
                PreqNode[] nodes = courses.Select(c => new PreqNode(c)).ToArray();
                string result = JsonConvert.SerializeObject(nodes);
                return Content(result);
            }
        }
    }
}
