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
            List<Course> courses;
            try
            {
                result = MauiWrapper.GetCourse(college);
                if(string.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get course from college: " + college);
                    return false;// TODO: more thorough logging
                }

                courses = new JavaScriptSerializer().Deserialize<List<Course>>(result);
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
                            CourseName = mauiCourse.CourseName,
                            CatalogDescription = mauiCourse.CatalogDescription,
                            CourseNumber = mauiCourse.CourseNumber,
                            CreditHours = mauiCourse.CreditHours
                        };
                        if(!db.Courses.Any(c => c.CourseName == course.CourseName))
                        {
                            db.Courses.Add(course);
                            //MauiSection.createPrerequesties(course);
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

        public static bool addPrerequesiteInformationToAllCourses()
        {
            try
            {
                using (var db = new DataContext())
                {
                    List<Course> courses1 = db.Courses.Where(c => c.CourseNumber != null).ToList();
                    foreach (Course course in courses1)
                    {
                        MauiSection.createPrerequesties(course);
                    }
                    db.SaveChanges();
                }

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}