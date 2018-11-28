namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFieldLength_ExternalReconciliation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ExternalReconciliations", "SearchFields", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ExternalReconciliations", "SearchFields", c => c.String(maxLength: 1000));
        }
    }
}
