using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace UI_Scheduler_Tool.Maui
{
    public static class MauiWrapper
    {
        public static string GetCourse(string course)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + course;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetMinors()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/minors";
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetProgramsOfStudyByNatKey(string natKey)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/college/nk/" + natKey;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetProgramOfStudyByProgramNatKey(string natKey)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/by-program/" + natKey;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetProgramOfStudyByID(string id)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/program-of-study/" + id;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetCurrentSession()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions/current";
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetAllSessions()
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sessions";
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetSections(int sessionId, string couseSubject, string courseNumber)
        {
            // TODO: do we want to make the page size and exclude vairables configurable? (right now they are set to max and none)
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections?json={{sessionId: {0}, courseSubject: '{1}', courseNumber: '{2}'}}&pageStart=0&pageSize=2147483647&",
                sessionId, couseSubject, courseNumber);
            return MauiHelper.GetJsonFromURL(url);
        }
    }
}