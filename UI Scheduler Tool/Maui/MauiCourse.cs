using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

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

        public static string GetJSON(string course)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + course;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static JToken FastGetSingleCoures(string courseNumber)
        {
            string result = MauiWrapper.GetCourse(courseNumber);
            if(result[0] == '[')
            {
                return JArray.Parse(result)[0];
            }
            else
            {
                return JToken.Parse(result);
            }
        }

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
            List<MauiCourse> courses = null;
            try
            {
                string result = MauiWrapper.GetCourse(course);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get course from college: " + course);
                    return courses;// TODO: more thorough logging
                }

                // sometimes we get an array which will contain data like this
                /*
                 * [
                 *  {
                 *      <data>
                 *  }
                 * ]
                 */
                // this will cause the deserializer to fail when parsing a list instead of
                // a single course
                // to avoid this just check if it is a list first then call the appropriate
                // deserializer call
                if (result[0] == '[')
                {
                    courses = new JavaScriptSerializer().Deserialize<List<MauiCourse>>(result);
                }
                else
                {
                    courses = new List<MauiCourse>()
                    {
                       new JavaScriptSerializer().Deserialize<MauiCourse>(result)
                    };
                }
                
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting courses from maui: " + e.Message);
            }
            return courses;
        }

        public void GetSubjectAndNumber(out string subject, out string number)
        {
            String[] parts = courseNumber.Split(':');
            subject = parts[0];
            number = parts[1];
        }
    }
}