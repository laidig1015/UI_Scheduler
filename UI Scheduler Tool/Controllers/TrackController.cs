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

        public ActionResult GetEFANodes(int efaId)
        {
            using (var db = new DataContext())
            {
                EFA efa = db.EFAs.Where(e => e.ID == efaId).FirstOrDefault();
                if (efa == null)
                {
                    return Content(string.Empty);
                }
                else
                {
                    List<JNode> nodes = db.EFACourses.Where(e => e.EFAID == efa.ID).Select(e => new JNode(e.Course) { type = e.EFAType }).ToList();
                    string result = JsonConvert.SerializeObject(nodes);
                    return Content(result);
                }
            }
        }

        public ActionResult GetTrackNodes(int trackId)
        {
            using (var db = new DataContext())
            {
                Track track = db.Tracks.Where(t => t.ID == trackId).FirstOrDefault();
                if (track == null)
                {
                    return Content(string.Empty);
                }
                else
                {
                    List<JNode> nodes = track.Curricula.Select(c => new JNode(c.Course) { index = c.SemesterIndex }).ToList();
                    string result = JsonConvert.SerializeObject(nodes);
                    return Content(result);
                }
            }
        }
    }
}
