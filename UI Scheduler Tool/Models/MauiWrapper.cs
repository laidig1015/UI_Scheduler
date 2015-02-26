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
        public static string getCourse(string course_number)
        {

            string result = "Not Found";
            string test_course_number = "55:032";
            string mainUrl = "https://api.maui.uiowa.edu/maui/api/pub/registrar/course/" + course_number;
            WebRequest courseRequest;
            courseRequest = WebRequest.Create(mainUrl);
            Stream objStream;
            objStream = courseRequest.GetResponse().GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);
            using(StreamReader r = new StreamReader(courseRequest.GetResponse().GetResponseStream()))
            {
                string sLine = String.Empty;
                int i = 0;
                while (sLine != null)
                {
                    i++;
                    sLine = objReader.ReadLine();
                    if (sLine != null)
                    {
                        Console.WriteLine("{0}:{1}", i, sLine);
                        result += sLine + '\n';
                    }
                }
                //Console.ReadLine();
                return result;
            }
            //return result;
        }

    }
}