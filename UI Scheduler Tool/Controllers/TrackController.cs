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

        public ActionResult GetCurriculumNodes(string trackName)
        {
            using (var db = new DataContext())
            {
                JPreqNode[] nodes = db.Tracks.Where(t => t.ShortName.Equals(trackName)).Single()
                                     .Curricula.Select(c => new JPreqNode(c.Course) { index = c.SemesterIndex }).ToArray();
                string result = JsonConvert.SerializeObject(nodes);
                return Content(result);
            }
        }
    }
}
