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
        public ActionResult Builder()
        {
            return View();
        }

        public ActionResult GetEFASeed()
        {
            JEFASeedData seed = new JEFASeedData();
            if (!seed.Load())
            {
                return Content(string.Empty);
            }
            else
            {
                string result = JsonConvert.SerializeObject(seed);
                return Content(result);
            }
        }

        public ActionResult GetCurriculumNodes(string trackName)
        {
            using (var db = new DataContext())
            {
                Track track = db.Tracks.Where(t => t.ShortName.Equals(trackName)).FirstOrDefault();
                if (track == null)
                {
                    return Content(string.Empty);
                }
                else
                {
                    JPreqNode[] nodes = track.Curricula.Select(c => new JPreqNode(c.Course) { index = c.SemesterIndex }).ToArray();
                    string result = JsonConvert.SerializeObject(nodes);
                    return Content(result);
                }
            }
        }
    }
}
