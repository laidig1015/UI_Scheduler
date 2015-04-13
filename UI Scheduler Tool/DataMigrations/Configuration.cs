namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using UI_Scheduler_Tool.Models;

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
            //AddTracksAndCurriculum(context);// TODO!!!
        }

        private void AddTreeBuildTest(UI_Scheduler_Tool.Models.DataContext context)
        {
            // root courses
            Course rootReq = new Course { CourseName = "RootRequired", CourseNumber = "ROOT:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course rootOptA = new Course { CourseName = "RootOptionalA", CourseNumber = "ROOT:1000", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course rootOptB = new Course { CourseName = "RootOptionalB", CourseNumber = "ROOT:1001", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            // easy preq
            Course easy = new Course { CourseName = "Easy", CourseNumber = "EZ:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            // advanced preq
            Course advancedA = new Course { CourseName = "AdvancedA", CourseNumber = "ADV:0000", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course advancedB = new Course { CourseName = "AdvancedB", CourseNumber = "ADV:0001", CatalogDescription = "TREE_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            context.SaveChanges();

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
            Course N0A = new Course { CourseName = "N0A", CourseNumber = "N0:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course N0B = new Course { CourseName = "N0B", CourseNumber = "N0:0002", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course N0C = new Course { CourseName = "N0C", CourseNumber = "N0:0003", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            // level 1
            Course N1A = new Course { CourseName = "N1A", CourseNumber = "N1:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course N1B = new Course { CourseName = "N1B", CourseNumber = "N1:0002", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course N1C = new Course { CourseName = "N1C", CourseNumber = "N1:0003", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            // level 2
            Course N2A = new Course { CourseName = "N2A", CourseNumber = "N2:0001", CatalogDescription = "PREQ_TEST", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            context.Courses.AddOrUpdate(
                N0A, N0B, N0C,
                N1A, N1B, N1C,
                N2A
                );

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
            context.Tracks.AddOrUpdate(
                new Track { Name = "TEST" }
                );
            context.SaveChanges();
        }
    }
}
