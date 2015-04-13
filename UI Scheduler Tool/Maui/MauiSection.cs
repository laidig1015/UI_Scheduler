using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Linq;
using UI_Scheduler_Tool.Models;

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
        public string SectionType { get; set; }
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
        public int SessionID { get; set; }

        public static string GetAllSections(int sectionID)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/" + sectionID;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetSectionSearch(int sessionId, string couseSubject, string courseNumber)
        {
            // TODO: do we want to make the page size and exclude vairables configurable? (right now they are set to max and none)
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections?json={{sessionId: {0}, courseSubject: '{1}', courseNumber: '{2}'}}&pageStart=0&pageSize=2147483647&",
                sessionId, couseSubject, courseNumber);
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetSectionByRelation(int sectionID, int courseIdentityID, SectionRelationships r, SectionTypes t)
        {
            // TODO: do we want to make the page size and exclude vairables configurable? (right now they are set to max and none)
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/{0}/{1}/{2}/{3}?pageStart=0&pageSize=2147483647&",
                sectionID, courseIdentityID, r.ToURLString(), t.ToURLString());
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetFromLegacy(int legacySectionID, string subject)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/" + legacySectionID + "/" + subject;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetFromLegacy(int legacySectionID, string subject, string course, bool isSimple)
        {
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/{0}/{1}/{2}/simple={3}&",
                legacySectionID, subject, course, isSimple);
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetFromLegacy(int legacySectionID, string subject, string course, string section, bool isSimple)
        {
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/{0}/{1}/{2}/{3}/simple={4}&",
                legacySectionID, subject, course, section, isSimple);
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetJSON(int sessionID, string combinedCourseNumber, bool isSimple)
        {
            string[] parts = combinedCourseNumber.Split(':');
            if (parts.Length < 2)
            {
                return string.Empty;
            }
            else if (parts.Length == 2)
            {
                return GetFromLegacy(sessionID, parts[0], parts[1], isSimple);
            }
            else
            {
                return GetFromLegacy(sessionID, parts[0], parts[1], parts[2], isSimple);
            }
        }

        public static string GetEnrollment(int sessionID, string[] sectionIDs)
        {
            StringBuilder urlBuilder = new StringBuilder("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/enrollments-n-status/" + sessionID + "/");
            foreach(string s in sectionIDs)
            {
                urlBuilder.Append(s);
                urlBuilder.Append(',');
            }
            return MauiHelper.GetJsonFromURL(urlBuilder.ToString());
        }

        public static string GetFromCourseIdentity(int sectionID, int courseIdenityID)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/" + sectionID + "/" + courseIdenityID;
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetFromRelationship(int sectionID, int courseIdenityID, SectionRelationships r)
        {
            string url = String.Format("https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/related/{0}/{1}/{2}",
                r.AliasUnrelatedToRelated().ToURLString(), sectionID, courseIdenityID);
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetTimesAndLocation(int sectionID)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/" + sectionID + "/times-and-locations";
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetInstructors(int sectionID)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/" + sectionID + "/instructors";
            return MauiHelper.GetJsonFromURL(url);
        }

        public static string GetRestrictions(int sectionID)
        {
            string url = "https://api.maui.uiowa.edu/maui/api/pub/registrar/sections/restrict-enrollments/" + sectionID;
            return MauiHelper.GetJsonFromURL(url);
        }

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
                //https://api.maui.uiowa.edu/maui/pub/webservices/documentation.page
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting sections from maui: " + e.Message);
            }
            return sections;
        }


        public static string createPrerequesties(Course course)
        {
            string prereq = null;
            if (course == null)
            {
                return null;
            }

            try
            {
                string subject, number;
                course.GetSubjectAndNumber(out subject, out number);
                string result = MauiWrapper.GetSections(course.LastTaughtID, subject, number);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get section from course (sessionId: {0} {1}:{2})", course.LastTaughtID, subject, number);
                    //return sections;// TODO: more thorough logging
                }
                //sections = JObject.Parse(result)["payload"].Select(token => FromToken(token)).ToList();
                JToken token = JObject.Parse(result)["payload"];

                JToken preToken = token[0];
               // JToken preToken = token["timeAndLocations"][0];
                string preString = (string)preToken["prerequisite"];
                char[] delimiters = { ' ' };
                string[] splitString = preString.Split(delimiters);

                List<string> prereqList = splitString.ToList<string>();
                int orIndex;
                int andIndex;

                while (prereqList.Count != 0)
                {
                    //Check for an "Or" Relationship
                    orIndex = prereqList.IndexOf("or");
                    if (orIndex >= 2)
                    {
                        //Grab Optional 1
                        string course1 = prereqList[orIndex - 2];
                        string legacy1 = prereqList[orIndex - 1];
                        string course2 = prereqList[orIndex + 1];
                        string legacy2 = prereqList[orIndex + 2];
                        //createPrereqEdge(number, course1, true);
                        //createPrereqEdge(number, course2, true);
                        //GET the Actual Corses Objects
                        using (var db = new DataContext())
                        {
                            //db.PreqEdges.Add(new PreqEdge { Parent = main, Child = reference, IsRequired = true });
                            List<Course> courses1 = db.Courses.Where(c => c.CourseNumber == course1).ToList();
                            List<Course> courses2 = db.Courses.Where(c => c.CourseNumber == course2).ToList();
                            if (courses1.Count > 0)
                            {
                                Course childCourse = courses1[0];
                                createPrereqEdge(course, childCourse, true);
                            }
                            if (courses2.Count > 0)
                            {
                                Course childCourse = courses2[0];
                                createPrereqEdge(course, childCourse, true);
                            }
                            // db.SaveChanges();
                        }

                        prereqList.Remove(course1);
                        prereqList.Remove(course2);
                        prereqList.Remove(legacy1);
                        prereqList.Remove(legacy2);
                        prereqList.Remove("or");
                    }
                    andIndex = prereqList.IndexOf("and");
                    if (andIndex >= 2)
                    {
                        string course1 = prereqList[andIndex - 2];
                        string legacy1 = prereqList[andIndex - 1];
                        //createPrereqEdge(number, course1, false);
                        using (var db = new DataContext())
                        {
                            //db.PreqEdges.Add(new PreqEdge { Parent = main, Child = reference, IsRequired = true });
                            List<Course> courses1 = db.Courses.Where(c => c.CourseNumber == course1).ToList();
                            //List<Course> courses2 = db.Courses.Where(c => c.CourseNumber == course2).ToList();
                            if (courses1.Count > 0)
                            {
                                Course childCourse = courses1[0];
                                createPrereqEdge(course, childCourse, true);
                            }
                            // db.SaveChanges();
                        }
                        prereqList.Remove(course1);
                        prereqList.Remove(legacy1);
                        prereqList.Remove("and");
                    }

                    if (prereqList.Count >= 2)
                    {
                        string course1 = prereqList[0];
                        string legacy1 = prereqList[1];
                        using (var db = new DataContext())
                        {
                            //db.PreqEdges.Add(new PreqEdge { Parent = main, Child = reference, IsRequired = true });
                            List<Course> courses1 = db.Courses.Where(c => c.CourseNumber == course1).ToList();
                            //List<Course> courses2 = db.Courses.Where(c => c.CourseNumber == course2).ToList();
                            if (courses1.Count > 0)
                            {
                                Course childCourse = courses1[0];
                                createPrereqEdge(course, childCourse, false);
                            }
                            db.SaveChanges();
                        }
                        prereqList.Remove(course1);
                        prereqList.Remove(legacy1);
                    }

                    if (prereqList.Count < 2 && prereqList.Count > 0)
                    {
                        prereqList.Clear();
                    }
                }
                //https://api.maui.uiowa.edu/maui/pub/webservices/documentation.page
                prereq = preString;
            }
            catch (Exception e)// TODO: BAD!
            {
                Console.Error.WriteLine("Error getting sections from maui: " + e.Message);
            }

            return prereq;
        }

        private static bool createPrereqEdge(Course main, Course reference, bool optional)
        {
             using(var db = new DataContext())
            {
                PreqEdge edge = new PreqEdge()
                {
                    Parent = main,
                    Child = reference,
                    IsRequired = optional
                };

                if (!db.PreqEdges.Any(c => c.Parent == edge.Parent && c.Child == edge.Child))
                {
                    db.PreqEdges.Add(edge);
                    //MauiSection.createPrerequesties(course);
                }
                //db.PreqEdges.Add(new PreqEdge { Parent = main, Child = reference, IsRequired = !(optional) });
                //List<Course> courses = db.Courses.Where(c => c.CourseNumber == "055:1742").ToList();
                db.SaveChanges();
            }

            return true;
        }

        public static Boolean IsClassInFallSemester(Course course)
        {
            try
            {
                string subject, number;
                course.GetSubjectAndNumber(out subject, out number);
                string result = MauiWrapper.GetSections(56, subject, number);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get section from course (sessionId: {0} {1}:{2})", course.LastTaughtID, subject, number);
                    //return sections;// TODO: more thorough logging
                }
                //sections = JObject.Parse(result)["payload"].Select(token => FromToken(token)).ToList();
                JToken token = JObject.Parse(result)["payload"];

                JToken preToken = token[0];
            }
            catch
            {
                return false;
            }
            return true;
        }


        public static Boolean IsClassInSpringSemester(Course course)
        {
            try
            {
                string subject, number;
                course.GetSubjectAndNumber(out subject, out number);
                string result = MauiWrapper.GetSections(59, subject, number);
                if (String.IsNullOrEmpty(result))
                {
                    Console.Error.WriteLine("Unable to get section from course (sessionId: {0} {1}:{2})", course.LastTaughtID, subject, number);
                    //return sections;// TODO: more thorough logging
                }
                //sections = JObject.Parse(result)["payload"].Select(token => FromToken(token)).ToList();
                JToken token = JObject.Parse(result)["payload"];

                JToken preToken = token[0];
            }
            catch
            {
                return false;
            }
            return true;
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
                SectionType = (string)token["sectionType"],
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
                Offsite = offsite,
                SessionID = (int)token["session"]
            };
            return section;
        }
    }
}