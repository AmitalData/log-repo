namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteShipmentComputedFields : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ShipmentComputedFields", "ImportDeclarationDate");
            DropColumn("dbo.ShipmentComputedFields", "ImportDeclarationNumber");
            DropColumn("dbo.ShipmentComputedFields", "DeliveryToCity");
            DropColumn("dbo.ShipmentComputedFields", "ContainsDangerousGoods");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShipmentComputedFields", "ContainsDangerousGoods", c => c.Boolean(nullable: false));
            AddColumn("dbo.ShipmentComputedFields", "DeliveryToCity", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "ImportDeclarationNumber", c => c.String());
            AddColumn("dbo.ShipmentComputedFields", "ImportDeclarationDate", c => c.DateTime());
        }
    }
}
