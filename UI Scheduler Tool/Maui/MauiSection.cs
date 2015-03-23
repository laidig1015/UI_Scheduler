using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;

namespace UI_Scheduler_Tool.Maui
{
    public class MauiSection
    {
        // TODO: we didn't copy all of the json data just yet because
        // we don't need it all now and we are lazy we might need it 
        // later thought
        // right now we only copy the stuff we need for the CourseSection
        // model

        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
        public string Hours { get; set; }
        public string ManagementType { get; set; }
        public string MaxEnroll { get; set; }
        public int CurrentEnroll { get; set; }
        public string MaxUnreservedEnroll { get; set; }
        public int CurrentUnreservedEnroll { get; set; }
        public bool UnlimitedEnroll { get; set; }
        public bool IsIndependentStudy { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Recurrence { get; set; }
        public string Room { get; set; }
        public string Building { get; set; }
        public bool ArrangedTime { get; set; }
        public bool ArrangedLocation { get; set; }
        public bool Offsite { get; set; }

        public static List<MauiSection> Get(MauiCourse course)
        {
            if (course == null)
            {
                return null;
            }
            List<MauiSection> sections = null;
            try
            {
                string subject, number;
                course.GetSubjectAndNumber(out subject, out number);
                string result = MauiWrapper.GetSections(course.lastTaughtId, subject, number);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get section from course (sessionId: {0} {1}:{2})", course.lastTaughtId, subject, number);
                    return sections;// TODO: more thorough logging
                }
                sections = JObject.Parse(result)["payload"].Select(token => FromToken(token)).ToList();
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting sections from maui: " + e.Message);
            }
            return sections;
        }

        private static MauiSection FromToken(JToken token)
        {
            // time and location variables
            JToken timeLoc = token["timeAndLocations"][0];// TODO: error if we have more 0 or more time and locations
            string start = (string)timeLoc["startTime"];
            string end = (string)timeLoc["endTime"];
            string recurence = (string)timeLoc["recurrence"];
            string building = (string)timeLoc["building"];
            string room = (string)timeLoc["room"];
            bool arrangedLoc = (bool)timeLoc["arrangedLocation"];
            bool arrangedTime = (bool)timeLoc["arrangedTime"];
            bool offsite = (bool)timeLoc["offsite"];

            MauiSection section = new MauiSection()
            {
                BeginDate = DateTime.Parse((string)token["beginDate"]),
                EndDate = DateTime.Parse((string)token["endDate"]),
                Status = (string)token["status"],
                Type = (string)token["sectionType"],
                Hours = (string)token["hours"],
                ManagementType = (string)token["managementType"],
                MaxEnroll = (string)token["maxEnroll"],
                CurrentEnroll = (int)token["currentEnroll"],
                MaxUnreservedEnroll = (string)token["maxUnreservedEnroll"],
                CurrentUnreservedEnroll = (int)token["currentUnreservedEnroll"],
                UnlimitedEnroll = (bool)token["unlimitedEnroll"],
                IsIndependentStudy = (bool)token["isIndependentStudySection"],
                StartTime = start,
                EndTime = end,
                Recurrence = recurence,
                Building = building,
                Room = room,
                ArrangedLocation = arrangedLoc,
                ArrangedTime = arrangedTime,
                Offsite = offsite
            };
            return section;
        }
    }
}