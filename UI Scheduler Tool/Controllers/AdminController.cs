using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using UI_Scheduler_Tool.Maui;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult FilterOldClasses()
        {
            using (var db = new DataContext())
            {
                Maui.MauiScripts.filterDatabase(db);
            }
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabMathCourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("MATH");
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabECECourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("ECE");
            return RedirectToAction("AdminPage");
        }

        public ActionResult GrabCSCourses()
        {
            Maui.MauiScripts.PopulateCourseFromCollege("CS");
            return RedirectToAction("AdminPage");
        }

        public ActionResult addPrerequistes()
        {
            using (var db = new DataContext())
            {
                Maui.MauiScripts.addPrerequesiteInformationToAllCourses(db);
            }
            return RedirectToRoute("AdminPage");
        }

        public ActionResult ViewCurrentCourses()
        {
            using (var db = new DataContext())
            {
                List<Course> all = db.Courses.ToList();
                //PreqTable table = new PreqTable(all);
                ViewBag.courses = all;
            }

            return View();
        }

        public ActionResult AdminPage()
        {
            return View();
        }
        
    }
}
