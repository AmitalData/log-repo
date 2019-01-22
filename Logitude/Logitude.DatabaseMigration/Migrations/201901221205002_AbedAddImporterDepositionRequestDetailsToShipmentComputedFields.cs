namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AbedAddImporterDepositionRequestDetailsToShipmentComputedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ShipmentComputedFields", "IsDepositionRequired", c => c.Boolean(nullable: false));

            AddColumn("dbo.ShipmentComputedFields", "ImporterDepositionRequestDetails", c => c.String(maxLength: 100, unicode: false));

 
        }
        
        public override void Down()
        {
          
            DropColumn("dbo.ShipmentComputedFields", "ImporterDepositionRequestDetails");
            DropColumn("dbo.ShipmentComputedFields", "IsDepositionRequired");
        }
    }
}
