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
        public static string getCourse(string course)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + course;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Http.Get;
            request.Accept = "application/json";
            using(StreamReader r = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                string result = r.ReadToEnd();
                return result;
            }
        }

    }
}