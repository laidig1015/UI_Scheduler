namespace UI_Scheduler_Tool.DataMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SwapForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PreqEdges", name: "ChildID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.PreqEdges", name: "ParentID", newName: "ChildID");
            RenameColumn(table: "dbo.PreqEdges", name: "__mig_tmp__0", newName: "ParentID");
            RenameIndex(table: "dbo.PreqEdges", name: "IX_ChildID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.PreqEdges", name: "IX_ParentID", newName: "IX_ChildID");
            RenameIndex(table: "dbo.PreqEdges", name: "__mig_tmp__0", newName: "IX_ParentID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PreqEdges", name: "IX_ParentID", newName: "__mig_tmp__0");
            RenameIndex(table: "dbo.PreqEdges", name: "IX_ChildID", newName: "IX_ParentID");
            RenameIndex(table: "dbo.PreqEdges", name: "__mig_tmp__0", newName: "IX_ChildID");
            RenameColumn(table: "dbo.PreqEdges", name: "ParentID", newName: "__mig_tmp__0");
            RenameColumn(table: "dbo.PreqEdges", name: "ChildID", newName: "ParentID");
            RenameColumn(table: "dbo.PreqEdges", name: "__mig_tmp__0", newName: "ChildID");
        }
    }
}
