namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using UI_Scheduler_Tool.Models;
    using UI_Scheduler_Tool.Models.Extensions;
    using UI_Scheduler_Tool.Maui;
    using Newtonsoft.Json.Linq;

    // Commands to seed DB:
    // Enable-Migrations -ContextTypeName UI_Scheduler_Tool.Models.DataContext -MigrationsDirectory:DataMigrations
    // Add-Migration -configuration UI_Scheduler_Tool.DataMigrations.Configuration <comment>
    // Update-Database -configuration UI_Scheduler_Tool.DataMigrations.Configuration -Verbose

    internal sealed class Configuration : DbMigrationsConfiguration<DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataMigrations";
        }

        protected override void Seed(DataContext context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();
            AddTracksAndCurriculum(context);
            //AddTrackEFACourses(context);
            Maui.MauiScripts.addPrerequesiteInformationToAllCourses(context);
        }

        public static void AddTracksAndCurriculum(DataContext context)
        {
            Track ece = new Track { ShortName = "ECE", Name = "Computer Electrical Engineering" };
            Track ee = new Track { ShortName = "EE", Name = "Electrical Engineering" };
            ece.Add(context);
            ee.Add(context);
            context.SaveChanges();

            #region ECE Track Courses
            string[] eceMatrix = 
            {
                // string is formated as
                // <Course Number>,<Is Offered In Fall>,<Is Offered In Spring>|...
                "MATH:1550,T,T|ENGR:1100,T,F|CHEM:1110,T,T|RHET:1030,T,T|ENGR:1000,T,F",
                "MATH:1560,T,T|ENGR:1300,T,T|PHYS:1611,T,T|MATH:2550,T,T",

                "MATH:2560,T,T|PHYS:1612,T,T|ENGR:2110,T,T|ENGR:2120,T,T|ENGR:2130,T,T",
                "MATH:3550,T,T|ECE:2400,F,T|ECE:2410,F,T|ENGR:2730,T,T",

                "STAT:2020,T,T|ECE:3320,T,F|CS:2210,T,T|ECE:3330,T,F|ECE:3700,T,F|ECE:3000,T,F",
                "CS:2230,T,T|ECE:3350,F,T|ECE:3360,F,T",

                "ECE:4880,T,T|CS:3330,T,T",
                "ECE:4890,T,T"
            };
            #endregion

            #region EE Track Courses
            string[] eeMatrix =
            {
                "MATH:1550,T,T|ENGR:1100,T,F|CHEM:1110,T,T|RHET:1030,T,T|ENGR:1000,T,F",
                "MATH:1560,T,T|ENGR:1300,T,T|PHYS:1611,T,T|MATH:2550,T,T",

                "MATH:2560,T,T|PHYS:1612,T,T|ENGR:2110,T,T|ENGR:2120,T,T|ENGR:2130,T,T",
                "MATH:3550,T,T|ECE:2400,F,T|ECE:2410,F,T|ENGR:2730,T,T",

                "STAT:2020,T,T|ECE:3320,T,F|ECE:3400,T,F|ECE:3410,T,F|ECE:3700,T,F|ECE:3000,T,F",
                "ECE:3500,F,T|ECE:3600,F,T|ECE:3720,F,T",

                "ECE:4880,T,T",
                "ECE:4890,T,T"
            };
            #endregion

            BuildCurriculum(context, ece, eceMatrix);
            BuildCurriculum(context, ee, eeMatrix);

            context.SaveChanges();
        }

        public static void AddTrackEFACourses(DataContext context)
        {
            Track eceTrack = context.Tracks.Where(t => t.ShortName.Equals("ECE")).Single();
            Track eeTrack = context.Tracks.Where(t => t.ShortName.Equals("EE")).Single();

            #region ECE Courses
            string eceBredthCourses = @"ECE:3540|
                                        ECE:3400|
                                        ECE:3410|
                                        ECE:3720|
                                        ECE:3600|
                                        ECE:3500";

            string eceDepthCourses =  @"ECE:5220|
                                        ECE:5300|
                                        ECE:5310|
                                        ECE:5320|
                                        ECE:5330|
                                        ECE:5380|
                                        ECE:5450|
                                        ECE:5460|
                                        ECE:5480|
                                        ECE:5520|
                                        ECE:5530|
                                        ECE:5800|
                                        ECE:5810|
                                        ECE:5820|
                                        ECE:5830|
                                        ECE:5995|
                                        CS:3820|
                                        CS:3620|
                                        CS:4340|
                                        CS:4330|
                                        CS:4400|
                                        CS:4420|
                                        CS:4440|
                                        CS:4520|
                                        CS:4640|
                                        CS:4700|
                                        CS:4350|
                                        CS:4980";
            #endregion

            #region EE Courses
            string eeBredthCourses = @"CS:2210|
                                        CS:3330|
                                        ECE:3360|
                                        ECE:3330|
                                        ECE:3350|
                                        ECE:3540";

            string eeDepthCourses = @"ECE:5310|
                                    ECE:5410|
                                    ECE:5420|
                                    ECE:5430|
                                    ECE:5450|
                                    ECE:5460|
                                    ECE:5480|
                                    ECE:5500|
                                    ECE:5520|
                                    ECE:5530|
                                    ECE:5600|
                                    ECE:5430|
                                    ECE:5630|
                                    ECE:5640|
                                    ECE:5700|
                                    ECE:5720|
                                    ECE:5780|
                                    ECE:5790|
                                    ECE:5995";
            #endregion

            #region Shared Courses
            string sharedUpper = @"ECE:4728|
                                   ECE:4720";

            string sharedTech = @"IE:2500";

            string sharedUpperRule = "ECE>5000";
            string sharedTechnicalRule = "CS>3000";
            #endregion

            BuildTrackList(context, new Track[]{ eceTrack }, EFAType.BREDTH, eceBredthCourses);
            BuildTrackList(context, new Track[]{ eceTrack }, EFAType.DEPTH, eceDepthCourses);

            BuildTrackList(context, new Track[]{ eeTrack }, EFAType.BREDTH, eeBredthCourses);
            BuildTrackList(context, new Track[]{ eeTrack }, EFAType.DEPTH, eeDepthCourses);

            BuildTrackList(context, new Track[]{ eeTrack, eceTrack }, EFAType.UPPER, sharedUpper);
            BuildTrackList(context, new Track[]{ eeTrack, eceTrack }, EFAType.TECHNICAL, sharedTech);
            BuildTrackRules(context, new Track[]{ eeTrack, eceTrack }, EFAType.UPPER, sharedUpperRule);
            BuildTrackRules(context, new Track[]{ eeTrack, eceTrack }, EFAType.TECHNICAL, sharedTechnicalRule);

            context.SaveChanges();
        }

        private static void BuildCurriculum(DataContext context, Track t, string[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                string[] courseStrs = matrix[i].Split('|');
                for (int j = 0; j < courseStrs.Length; j++)
                {
                    string[] parts = courseStrs[j].Split(',');
                    JToken mauiCourse = MauiCourse.FastGetSingleCourse(parts[0]);// first part is the coures number
                    Course course = new Course
                    {
                        CourseName = (string)mauiCourse["title"],
                        CatalogDescription = (string)mauiCourse["catalogDescription"],
                        CourseNumber = parts[0],
                        CreditHours = (string)mauiCourse["creditHours"],
                        LastTaughtID = (int)mauiCourse["lastTaughtId"],
                        IsOfferedInFall = (parts[1][0] == 'T'),
                        IsOfferedInSpring = (parts[2][0] == 'T')
                    };
                    course = course.Add(context);
                    Curriculum curriculum = new Curriculum
                    {
                        Course = course,
                        Track = t,
                        SemesterIndex = i
                    };
                    curriculum.Add(context);
                }
            }
        }

        private static void BuildTrackList(DataContext context, Track[] tracks, EFAType type, string courses)
        {
            string[] coursesNumbers = courses.Split('|');
            foreach (string courseStr in coursesNumbers)
            {
                char[] ignore = { '\r', '\n', ' '};
                string number = courseStr.Trim(ignore);
                JToken token = Maui.MauiCourse.FastGetSingleCourse(number);
                if(token == null)
                {
                    continue;
                }
                Course course = new Course
                {
                    CourseName = (string)token["title"],
                    CatalogDescription = (string)token["catalogDescription"],
                    CourseNumber = number,
                    CreditHours = (string)token["creditHours"],
                    LastTaughtID = (int)token["lastTaughtId"],
                    IsOfferedInFall = true,// TODO FIX
                    IsOfferedInSpring = true
                };
                course = course.Add(context);
                foreach (Track t in tracks)
                {
                    TrackCourses trackCourse = new TrackCourses
                    {
                        Track = t,
                        Course = course,
                        EFAType = (int)type
                    };
                    trackCourse.Add(context);
                }
            }
            context.SaveChanges();
        }

        private static void BuildTrackRules(DataContext context, Track[] tracks, EFAType type, string rules)
        {

        }
    }
}
