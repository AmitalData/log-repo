namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixReferenceLengthInTransferLine_Samar : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AccountingTransferLines", "EntityReference", c => c.String(maxLength: 30, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AccountingTransferLines", "EntityReference", c => c.String(maxLength: 20, unicode: false));
        }
    }
}
