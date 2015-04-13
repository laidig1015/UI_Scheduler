namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedTrackShortName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tracks", "ShortName", c => c.String(nullable: false, maxLength: 16, unicode: false));
            CreateIndex("dbo.Tracks", "ShortName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Tracks", new[] { "ShortName" });
            DropColumn("dbo.Tracks", "ShortName");
        }
    }
}
