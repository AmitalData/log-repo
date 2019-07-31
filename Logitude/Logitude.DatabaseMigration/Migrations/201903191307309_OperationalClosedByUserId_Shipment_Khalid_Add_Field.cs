namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OperationalClosedByUserId_Shipment_Khalid_Add_Field : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "OperationalClosedByUserId", c => c.String(maxLength: 15, unicode: false));
            CreateIndex("dbo.Shipments", "OperationalClosedByUserId");
            AddForeignKey("dbo.Shipments", "OperationalClosedByUserId", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Shipments", "OperationalClosedByUserId", "dbo.Users");
            DropIndex("dbo.Shipments", new[] { "OperationalClosedByUserId" });
            DropColumn("dbo.Shipments", "OperationalClosedByUserId");
        }
    }
}
