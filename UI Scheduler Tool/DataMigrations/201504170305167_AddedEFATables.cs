namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedEFATables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EFAs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ShortName = c.String(nullable: false, maxLength: 16, unicode: false),
                        Name = c.String(maxLength: 64),
                        TrackID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: true)
                .Index(t => t.ShortName)
                .Index(t => t.TrackID);
            
            CreateTable(
                "dbo.EFACourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EFAID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        EFAType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.EFAs", t => t.EFAID, cascadeDelete: true)
                .Index(t => t.EFAID)
                .Index(t => t.CourseID);
            
            CreateTable(
                "dbo.TrackCourses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TrackID = c.Int(nullable: false),
                        CourseID = c.Int(nullable: false),
                        EFAType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Courses", t => t.CourseID, cascadeDelete: true)
                .ForeignKey("dbo.Tracks", t => t.TrackID, cascadeDelete: true)
                .Index(t => t.TrackID)
                .Index(t => t.CourseID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrackCourses", "TrackID", "dbo.Tracks");
            DropForeignKey("dbo.TrackCourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.EFACourses", "EFAID", "dbo.EFAs");
            DropForeignKey("dbo.EFACourses", "CourseID", "dbo.Courses");
            DropForeignKey("dbo.EFAs", "TrackID", "dbo.Tracks");
            DropIndex("dbo.TrackCourses", new[] { "CourseID" });
            DropIndex("dbo.TrackCourses", new[] { "TrackID" });
            DropIndex("dbo.EFACourses", new[] { "CourseID" });
            DropIndex("dbo.EFACourses", new[] { "EFAID" });
            DropIndex("dbo.EFAs", new[] { "TrackID" });
            DropIndex("dbo.EFAs", new[] { "ShortName" });
            DropTable("dbo.TrackCourses");
            DropTable("dbo.EFACourses");
            DropTable("dbo.EFAs");
        }
    }
}
