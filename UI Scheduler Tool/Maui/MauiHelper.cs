using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace UI_Scheduler_Tool.Maui
{
    public static class MauiHelper
    {
        public static string GetJsonFromURL(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Get;
                request.Accept = "application/json";
                using (StreamReader r = new StreamReader(request.GetResponse().GetResponseStream()))
                {
                    return r.ReadToEnd();
                }
            }
            catch(WebException)
            {
                return string.Empty;
            }
        }

        // EX: CS:1110
        public static bool TryParseSujectCourse(string combined, ref string subject, ref string course)
        {
            string[] parts = combined.Split(':');
            if (parts.Length >= 2)
            {
                subject = parts[0];
                course = parts[1];
                return true;
            }
            return false;
        }

        // EX: CS:1110:0AAA
        public static bool TryParseSujectCourseSection(string combined, ref string subject, ref string course, ref string section)
        {
            string[] parts = combined.Split(':');
            if(parts.Length >= 3)
            {
                subject = parts[0];
                course = parts[1];
                section = parts[2];
                return true;
            }
            return false;
        }
    }
}