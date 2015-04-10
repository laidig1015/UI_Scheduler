namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredsToCourse : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CourseName", c => c.String(nullable: false, maxLength: 64, unicode: false));
            AlterColumn("dbo.Courses", "CourseNumber", c => c.String(nullable: false, maxLength: 16, unicode: false));
            AlterColumn("dbo.Courses", "CreditHours", c => c.String(nullable: false, maxLength: 16, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "CreditHours", c => c.String(maxLength: 16, unicode: false));
            AlterColumn("dbo.Courses", "CourseNumber", c => c.String(maxLength: 16, unicode: false));
            AlterColumn("dbo.Courses", "CourseName", c => c.String(maxLength: 64, unicode: false));
        }
    }
}
