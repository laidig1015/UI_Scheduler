namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CourseName = c.String(maxLength: 64, unicode: false),
                        CatalogDescription = c.String(maxLength: 256),
                        CourseNumber = c.String(maxLength: 16, unicode: false),
                        CreditHours = c.String(maxLength: 16, unicode: false),
                        Occurence = c.Int(nullable: false),
                        LastTaughtID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.PreqEdges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentID = c.Int(nullable: false),
                        ChildID = c.Int(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Courses", t => t.ChildID)
                .ForeignKey("dbo.Courses", t => t.ParentID)
                .Index(t => t.ParentID)
                .Index(t => t.ChildID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PreqEdges", "ParentID", "dbo.Courses");
            DropForeignKey("dbo.PreqEdges", "ChildID", "dbo.Courses");
            DropIndex("dbo.PreqEdges", new[] { "ChildID" });
            DropIndex("dbo.PreqEdges", new[] { "ParentID" });
            DropTable("dbo.PreqEdges");
            DropTable("dbo.Courses");
        }
    }
}
