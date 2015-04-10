namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Courses", "CourseNumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Courses", new[] { "CourseNumber" });
        }
    }
}
