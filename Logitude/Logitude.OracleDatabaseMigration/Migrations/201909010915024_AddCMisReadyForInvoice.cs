namespace Logitude.OracleDatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCMisReadyForInvoice : DbMigration
    {
        public override void Up()
        {
            AddColumn("Customs.CourierMasters", "isReadyForInvoice", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Customs.CourierMasters", "isReadyForInvoice");
        }
    }
}
