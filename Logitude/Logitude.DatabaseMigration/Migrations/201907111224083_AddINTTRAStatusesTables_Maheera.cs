namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddINTTRAStatusesTables_Maheera : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.INTTRABookingStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //        })
            //    .PrimaryKey(t => t.Code);
            
            //CreateTable(
            //    "dbo.INTTRABookingTransStatuses",
            //    c => new
            //        {
            //            Code = c.String(nullable: false, maxLength: 4, unicode: false),
            //            Name = c.String(nullable: false, maxLength: 40, unicode: false),
            //            SearchFields = c.String(maxLength: 1000),
            //        })
            //    .PrimaryKey(t => t.Code);
            

            //AddColumn("dbo.Shipments", "INTTRABookingTransStatusCode", c => c.String(maxLength: 4, unicode: false));
            //AddColumn("dbo.Shipments", "INTTRABookingStatusCode", c => c.String(maxLength: 4, unicode: false));
            //CreateIndex("dbo.Shipments", "INTTRABookingTransStatusCode");
            //CreateIndex("dbo.Shipments", "INTTRABookingStatusCode");
            //AddForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatuses", "Code");
            //AddForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatuses", "Code");
        }
        
        public override void Down()
        {
        //    DropForeignKey("dbo.Shipments", "INTTRABookingTransStatusCode", "dbo.INTTRABookingTransStatus");
        //    DropForeignKey("dbo.Shipments", "INTTRABookingStatusCode", "dbo.INTTRABookingStatus");
        //    DropIndex("dbo.Shipments", new[] { "INTTRABookingStatusCode" });
        //    DropIndex("dbo.Shipments", new[] { "INTTRABookingTransStatusCode" });
        //    DropColumn("dbo.Shipments", "INTTRABookingStatusCode");
        //    DropColumn("dbo.Shipments", "INTTRABookingTransStatusCode");
        //    DropTable("dbo.INTTRABookingTransStatus");
        //    DropTable("dbo.INTTRABookingStatus");
        }
    }
}
