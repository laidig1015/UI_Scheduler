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
            AddTreeBuildTest(context);
            AddPreqChecktest(context);
            AddTracksAndCurriculum(context);
        }

        private void AddTreeBuildTest(UI_Scheduler_Tool.Models.DataContext context)
        {
            // root courses
            Course rootReq = new Course { CourseName = "RootRequired", CourseNumber = "ROOT:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course rootOptA = new Course { CourseName = "RootOptionalA", CourseNumber = "ROOT:1000", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course rootOptB = new Course { CourseName = "RootOptionalB", CourseNumber = "ROOT:1001", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };

            // easy preq
            Course easy = new Course { CourseName = "Easy", CourseNumber = "EZ:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };

            // advanced preq
            Course advancedA = new Course { CourseName = "AdvancedA", CourseNumber = "ADV:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course advancedB = new Course { CourseName = "AdvancedB", CourseNumber = "ADV:0001", CatalogDescription = "TREE_TEST", CreditHours = "0", LastTaughtID = 0 };

            Course.AddIgnoreRepeats(new List<Course>
                {
                    rootReq, rootOptA, rootOptB,
                    easy,
                    advancedA, advancedB
                }, context);

            context.PreqEdges.AddOrUpdate(
                // add level 0 edges
                new PreqEdge { Parent = rootReq, Child = easy, IsRequired = true },
                new PreqEdge { Parent = rootOptA, Child = easy, IsRequired = false },
                new PreqEdge { Parent = rootOptB, Child = easy, IsRequired = false },

                // add level 1 edges
                new PreqEdge { Parent = easy, Child = advancedA, IsRequired = true },
                new PreqEdge { Parent = easy, Child = advancedB, IsRequired = true }
                );
        }

        private void AddPreqChecktest(UI_Scheduler_Tool.Models.DataContext context)
        {
            // basic preq check nodes
            // level 0
            Course N0A = new Course { CourseName = "N0A", CourseNumber = "N0:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course N0B = new Course { CourseName = "N0B", CourseNumber = "N0:0002", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course N0C = new Course { CourseName = "N0C", CourseNumber = "N0:0003", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            // level 1
            Course N1A = new Course { CourseName = "N1A", CourseNumber = "N1:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course N1B = new Course { CourseName = "N1B", CourseNumber = "N1:0002", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            Course N1C = new Course { CourseName = "N1C", CourseNumber = "N1:0003", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };
            // level 2
            Course N2A = new Course { CourseName = "N2A", CourseNumber = "N2:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", LastTaughtID = 0 };

            Course.AddIgnoreRepeats(new List<Course>
                {
                    N0A, N0B, N0C,
                    N1A, N1B, N1C,
                    N2A
                }, context);

            // basic preq check nodes

            context.SaveChanges();

            context.PreqEdges.AddOrUpdate(
                // add level 0 edges
                new PreqEdge { Parent = N0A, Child = N1A, IsRequired = true },
                new PreqEdge { Parent = N0B, Child = N1A, IsRequired = true },
                new PreqEdge { Parent = N0C, Child = N1B, IsRequired = true },
                // add level 1 edges
                new PreqEdge { Parent = N1A, Child = N2A, IsRequired = true },
                new PreqEdge { Parent = N1B, Child = N2A, IsRequired = false },
                new PreqEdge { Parent = N1C, Child = N2A, IsRequired = true }
                );
        }

        private void AddTracksAndCurriculum(UI_Scheduler_Tool.Models.DataContext context)
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

        private void BuildCurriculum(UI_Scheduler_Tool.Models.DataContext context, Track t, string[] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                string[] courseStrs = matrix[i].Split('|');
                for (int c = 0; c < courseStrs.Length; c++)
                {
                    string[] parts = courseStrs[c].Split(',');
                    JToken mauiCourse = MauiCourse.FastGetSingleCoures(parts[0]);// first part is the coures number
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
