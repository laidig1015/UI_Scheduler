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

        public ActionResult GetCurriculum(string trackName)
        {
            using(var db = new DataContext())
            {
                //UI_Scheduler_Tool.DataMigrations.Configuration.AddTracksAndCurriculum(db);
                Maui.MauiScripts.addPrerequesiteInformationToAllCourses(db);
            }

            return View();

            //using (var db = new DataContext())
            //{
            //    Track track = db.Tracks.Where(t => t.ShortName.Equals(trackName)).Single();
            //    List<TrackMatrixNode>[] matrix = new List<TrackMatrixNode>[8];
            //    for (int i = 0; i < 8; i++)
            //    {
            //        matrix[i] = new List<TrackMatrixNode>();
            //    }
            //    List<Curriculum> curriculum = track.Curricula.ToList();
            //    foreach (Curriculum c in curriculum)
            //    {
            //        matrix[c.SemesterIndex].Add(new TrackMatrixNode(c.Course));
            //    }
            //    string result = JsonConvert.SerializeObject(matrix);
            //    return Content(result);
            //}
        }
    }
}
