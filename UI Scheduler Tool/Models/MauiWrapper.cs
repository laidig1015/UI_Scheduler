using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace UI_Scheduler_Tool.Models
{
    public static class MauiWrapper
    {
        public static string GetCourse(string course)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + course;
            return GetJsonFromURL(url);
        }

        public static string GetMinors()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/minors";
            return GetJsonFromURL(url);
        }

        public static string GetProgramsOfStudyByNatKey(string natKey)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/college/nk/" + natKey;
            return GetJsonFromURL(url);
        }

        public static string GetProgramOfStudyByProgramNatKey(string natKey)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/by-program/" + natKey;
            return GetJsonFromURL(url);
        }

        public static string GetProgramOfStudyByID(string id)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/" + id;
            return GetJsonFromURL(url);
        }

        public static string GetCurrentSession()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions/current";
            return GetJsonFromURL(url);
        }

        public static string GetAllSessions()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions";
            return GetJsonFromURL(url);
        }

        private static string GetJsonFromURL(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            using (StreamReader r = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string result = r.ReadToEnd();
                return result;
            }
        }
    }
}