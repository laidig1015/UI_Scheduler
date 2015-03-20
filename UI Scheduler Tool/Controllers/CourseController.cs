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
        public ActionResult Course()
        {
            return View();
        }

        public ActionResult Search(string college)
        {
            List<MauiCourse> mauiCourses = MauiCourse.GetCoursesFromCollege(college);
            if(mauiCourses == null)
            {
                // TODO: error here
                return PartialView("_CoursesPartial", new List<Course>());
            }

            var courses = mauiCourses.Select(mc => new Course
            {
                CourseName = mc.title,
                CatalogDescription = mc.catalogDescription,
                LastTaught = mc.lastTaught,
                CourseNumber = mc.courseNumber,
                LegacyCourseNumber = mc.legacyCourseNumber,
                CreditHours = mc.creditHours
            });
            return PartialView("_CoursesPartial", courses);
        }
    }
}
