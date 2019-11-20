namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InterestBasesTypeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.InterestBasesTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        Tenant = c.Int(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        CreatedByUserId = c.String(nullable: false, maxLength: 15, unicode: false),
                        UpdateDate = c.DateTime(nullable: false),
                        UpdatedByUserId = c.String(maxLength: 15, unicode: false),
                        SearchFields = c.String(),
                        Code = c.Int(nullable: false),
                        LocalName = c.String(maxLength: 256),
                        EnglishName = c.String(maxLength: 256, unicode: false),
                        Description = c.String(maxLength: 1024),
                        InActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.CreatedByUserId)
                .ForeignKey("dbo.Users", t => t.UpdatedByUserId)
                .Index(t => t.CreatedByUserId)
                .Index(t => t.UpdatedByUserId);
            
            //AddColumn("dbo.ShippingLines", "INTTRAUpdatesShipment", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Shipments", "INTTRABookingError", c => c.String(maxLength: 256, unicode: false));
            //AddColumn("dbo.Shipments", "INTTRALastBookingResponse", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InterestBasesTypes", "UpdatedByUserId", "dbo.Users");
            DropForeignKey("dbo.InterestBasesTypes", "CreatedByUserId", "dbo.Users");
            DropIndex("dbo.InterestBasesTypes", new[] { "UpdatedByUserId" });
            DropIndex("dbo.InterestBasesTypes", new[] { "CreatedByUserId" });
            //DropColumn("dbo.Shipments", "INTTRALastBookingResponse");
            //DropColumn("dbo.Shipments", "INTTRABookingError");
            //DropColumn("dbo.ShippingLines", "INTTRAUpdatesShipment");
            DropTable("dbo.InterestBasesTypes");
        }
    }
}
