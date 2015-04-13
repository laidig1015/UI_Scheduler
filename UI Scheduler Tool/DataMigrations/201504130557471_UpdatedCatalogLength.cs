namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedCatalogLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Courses", "CatalogDescription", c => c.String(maxLength: 4000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Courses", "CatalogDescription", c => c.String(maxLength: 256));
        }
    }
}
