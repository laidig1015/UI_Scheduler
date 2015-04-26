namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Courses", new[] { "CourseNumber" });
            CreateIndex("dbo.Courses", "CourseNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", new[] { "CourseNumber" });
            CreateIndex("dbo.Courses", "CourseNumber", unique: true);
        }
    }
}
