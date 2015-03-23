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

        public ActionResult SearchCourse(string course)
        {
            return PartialView("_CoursesPartial", Course.FromMauiCourses(MauiCourse.Get(course)));
        }
    }
}
