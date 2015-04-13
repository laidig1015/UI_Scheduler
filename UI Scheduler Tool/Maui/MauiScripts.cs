﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using UI_Scheduler_Tool.Models;
using Newtonsoft.Json.Linq;

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
                JArray mauiCourses = null;
                if (result[0] == '[')
                {
                    mauiCourses = JArray.Parse(result);
                }
                else
                {
                    mauiCourses = new JArray();
                    mauiCourses.Add(JToken.Parse(result));
                }
                courses = mauiCourses.Select(t =>
                    new Course
                    {
                        CourseName = (string)t["title"],
                        CourseNumber = (string)t["courseNumber"],
                        CreditHours = (string)t["creditHours"],
                        LastTaughtID = t["lastTaughtId"] == null ? 0 : (int)t["lastTaughtId"],
                        CatalogDescription = (string)t["catalogDescription"],
                        IsOfferedInFall = true,
                        IsOfferedInSpring = true
                    }).ToList();
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
                    Course.AddIgnoreRepeats(courses, db);
                }
            }
            catch (Exception e)// TODO: BAD!!!
            {
                Console.Error.WriteLine("Error writing db courses: " + e.Message);
                return false;
            }
            return true;
        }

        public static bool addPrerequesiteInformationToAllCourses(DataContext db)
        {
            try
            {
                List<Course> courses = db.Courses.ToList();
                foreach (Course course in courses)
                {
                    MauiSection.createPrerequesties(course, db);
                }
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return false;
            }
            return true;
        }
    }
}