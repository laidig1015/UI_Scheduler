namespace UI_Scheduler_Tool.UserMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 56),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.webpages_Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.webpages_RolesUserProfile",
                c => new
                    {
                        webpages_Roles_RoleId = c.Int(nullable: false),
                        UserProfile_UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.webpages_Roles_RoleId, t.UserProfile_UserId })
                .ForeignKey("dbo.webpages_Roles", t => t.webpages_Roles_RoleId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfile", t => t.UserProfile_UserId, cascadeDelete: true)
                .Index(t => t.webpages_Roles_RoleId)
                .Index(t => t.UserProfile_UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.webpages_RolesUserProfile", "UserProfile_UserId", "dbo.UserProfile");
            DropForeignKey("dbo.webpages_RolesUserProfile", "webpages_Roles_RoleId", "dbo.webpages_Roles");
            DropIndex("dbo.webpages_RolesUserProfile", new[] { "UserProfile_UserId" });
            DropIndex("dbo.webpages_RolesUserProfile", new[] { "webpages_Roles_RoleId" });
            DropTable("dbo.webpages_RolesUserProfile");
            DropTable("dbo.webpages_Roles");
            DropTable("dbo.UserProfile");
        }
    }
}
