using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using UI_Scheduler_Tool.Models;

namespace UI_Scheduler_Tool.Maui
{
    public static class MauiScripts
    {
        public static bool PopulateCourseFromCollege(string college)
        {
            string result;
            List<MauiCourse> courses;
            try
            {
                result = MauiWrapper.GetCourse(college);
                if(string.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get course from college: " + college);
                    return false;// TODO: more thorough logging
                }

                courses = new JavaScriptSerializer().Deserialize<List<MauiCourse>>(result);
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting courses from maui: " + e.Message);
                return false;// TODO: more thorough logging
            }

            try
            {
                using (var db = new DataContext())
                {
                    foreach (var mauiCourse in courses)
                    {
                        Course course = new Course()
                        {
                            CourseName = mauiCourse.title,
                            CatalogDescription = mauiCourse.catalogDescription,
                            CourseNumber = mauiCourse.courseNumber,
                            CreditHours = mauiCourse.creditHours
                        };
                        if(!db.Courses.Any(c => c.CourseName == course.CourseName))
                        {
                            db.Courses.Add(course);
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)// TODO: BAD!!!
            {
                Console.Error.WriteLine("Error writing db courses: " + e.Message);
                return false;
            }
            return true;
        }
    }
}