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
    public class CourseController : Controller
    {
        public ActionResult Search()
        {
            return View();
        }

        public ActionResult BlockSchedueler()
        {
            return View();
        }

        public ActionResult SearchCourse(string course)
        {
            using (var db = new DataContext())
            {
                List<Course> all = db.Courses.ToList();
                //PreqTable table = new PreqTable(all);
            }
            //return PartialView("_CoursesPartial", Course.FromMauiCourses(MauiCourse.Get(course)));
            //List<MauiCourse> courses = MauiCourse.Get(course);
            //// TEST
            //List<MauiSection> sections = MauiSection.Get(courses[0]);
            //return PartialView("_CoursesPartial", Course.FromMauiCourses(courses));
            return View();
        }

        public ActionResult SearchSection(string sectionSubject)
        {
            // TODO section is hard coded for now!!!
            return PartialView("_CourseSectionPartial", new MauiBlockCollection(MauiBlockSection.Load(59, sectionSubject)));
        }
    }
}
