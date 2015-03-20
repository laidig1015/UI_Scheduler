﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace UI_Scheduler_Tool.Maui
{
    public class MauiCourse
    {
        public string title { get; set; }
        public string catalogDescription { get; set; }
        public string lastTaught { get; set; }
        public int lastTaughtId { get; set; }
        public string lastTaughtCode { get; set; }
        public string courseNumber { get; set; }
        public string legacyCourseNumber { get; set; }
        public string creditHours { get; set; }// should be an int most of the time

        public static List<MauiCourse> Get(string course)
        {
            if (String.IsNullOrEmpty(course))
            {
                return null;
            }
            else
            {
                course = course.Trim();
            }
            string result;
            List<MauiCourse> courses = null;
            try
            {
                result = MauiWrapper.GetCourse(course);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get course from college: " + course);
                    return courses;// TODO: more thorough logging
                }

                courses = new JavaScriptSerializer().Deserialize<List<MauiCourse>>(result);
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting courses from maui: " + e.Message);
                return courses;// TODO: more thorough logging
            }
            return courses;
        }
    }
}