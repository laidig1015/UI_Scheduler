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
            //  This method will be called after migrating to the latest version.

            // root courses
            Course rootReq = new Course { CourseName = "RootRequired", CourseNumber = "ROOT:0000", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course rootOptA = new Course { CourseName = "RootOptionalA", CourseNumber = "ROOT:1000", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course rootOptB = new Course { CourseName = "RootOptionalB", CourseNumber = "ROOT:1001", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            // easy preq
            Course easy = new Course { CourseName = "Easy", CourseNumber = "EZ:0000", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            // advanced preq
            Course advancedA = new Course { CourseName = "AdvancedA", CourseNumber = "ADV:0000", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };
            Course advancedB = new Course { CourseName = "AdvancedB", CourseNumber = "ADV:0001", CreditHours = "0", Occurence = 0, LastTaughtID = 0 };

            context.Courses.AddOrUpdate(
                rootReq, rootOptA, rootOptB,
                easy,
                advancedA, advancedB
                );

            context.SaveChanges();

            context.PreqEdges.AddOrUpdate(
                // add level 1 edges
                new PreqEdge { Parent = rootReq, Child = easy, IsRequired = true },
                new PreqEdge { Parent = rootOptA, Child = easy, IsRequired = false },
                new PreqEdge { Parent = rootOptB, Child = easy, IsRequired = false },

                // add level 2 edges
                new PreqEdge { Parent = easy, Child = advancedA, IsRequired = true },
                new PreqEdge { Parent = easy, Child = advancedB, IsRequired = true }
                );

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
