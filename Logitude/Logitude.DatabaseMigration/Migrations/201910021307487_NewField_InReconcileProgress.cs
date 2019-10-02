namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewField_InReconcileProgress : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReconcileExternalPageLines", "InReconcileProgress");
        }
    }
}
