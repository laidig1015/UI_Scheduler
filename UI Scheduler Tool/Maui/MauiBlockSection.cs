using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace UI_Scheduler_Tool.Maui
{
    public class MauiBlockSection
    {
        public int SectionId { get; set; }
        public int SessionId { get; set; }
        public int CourseIdentityId { get; set; }
        public string CourseTitle { get; set; }
        public string SectionType { get; set; }
        public string Hours { get; set; }
        public string Recurence { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }


        public static List<MauiBlockSection> Load(int sectionId, string subjectCourse)
        {
            List<MauiBlockSection> sections = new List<MauiBlockSection>();
            try
            {
                string[] parts = subjectCourse.Split(':');
                if (parts == null || parts.Length < 2)
                {
                    return sections;
                }
                string result = MauiSection.GetSectionSearch(sectionId, parts[0], parts[1]);
                if (String.IsNullOrEmpty(result))
                {
                    return sections;
                }
                sections = JObject.Parse(result)["payload"].Select(token => FromToken(token)).ToList();
            }
            catch(Exception e)// TODO BAD!!!
            {
                Console.Error.WriteLine(e.Message);
            }
            return sections;
        }

        private static MauiBlockSection FromToken(JToken token)
        {
            JToken timeLoc = token["timeAndLocations"][0];
            MauiBlockSection section = new MauiBlockSection()
            {
                SectionId = (int)token["sectionId"],
                SessionId = (int)token["session"],
                CourseIdentityId = (int)token["courseIdentityId"],
                CourseTitle = (string)token["courseTitle"],
                SectionType = (string)token["sectionType"],
                Hours = (string)token["hours"],
                Recurence = (string)timeLoc["recurrence"],
                StartTime = (string)timeLoc["startTime"],
                EndTime = (string)timeLoc["endTime"]
            };
            return section;
        }
    }
}