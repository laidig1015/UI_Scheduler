namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using UI_Scheduler_Tool.Models;
    using UI_Scheduler_Tool.Maui;
    using Newtonsoft.Json.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<UI_Scheduler_Tool.Models.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"DataMigrations";
        }

        protected override void Seed(UI_Scheduler_Tool.Models.DataContext context)
        {
            AddTracksAndCurriculum(context);
            Maui.MauiScripts.addPrerequesiteInformationToAllCourses(context);
        }

        public static void AddTracksAndCurriculum(UI_Scheduler_Tool.Models.DataContext context)
        {
            Track ece = new Track { ShortName = "ECE", Name = "Computer Electrical Engineering" };
            Track ee = new Track { ShortName = "EE", Name = "Electrical Engineering" };
            context.Tracks.AddOrUpdate(ece, ee);
            context.SaveChanges();

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

            BuildCurriculum(context, ece, eceMatrix);
            BuildCurriculum(context, ee, eeMatrix);

            try
            {
                context.SaveChanges();
            }
            catch(DbEntityValidationException e)
            {
                Console.WriteLine("ERROR: " + e.Message);
            }
        }

        private static void BuildCurriculum(UI_Scheduler_Tool.Models.DataContext context, Track t, string[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                string[] courseStrs = matrix[i].Split('|');
                for (int c = 0; c < courseStrs.Length; c++)
                {
                    string[] parts = courseStrs[c].Split(',');
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
                    context.Courses.Add(Course.GetCourse(context, course));// gets or adds course
                    context.Curricula.Add(new Curriculum() { Course = course, Track = t, SemesterIndex = i });
                }
            }
        }
    }
}
