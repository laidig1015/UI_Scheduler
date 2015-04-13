namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCurriculum : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tracks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 64),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Curricula",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrackID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        SemesterIndex = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: true)
                .Index(t => t.TrackID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Curricula", "TrackID", "dbo.Tracks");
            DropForeignKey("dbo.Curricula", "CourseID", "dbo.Courses");
            DropIndex("dbo.Curricula", new[] { "CourseID" });
            DropIndex("dbo.Curricula", new[] { "TrackID" });
            DropTable("dbo.Curricula");
            DropTable("dbo.Tracks");
        }
    }
}
