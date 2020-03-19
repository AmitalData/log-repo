namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransportModeCodeToCarrierArea_Samar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarrierAreas", "TransportModeCode", c => c.String(nullable: true, maxLength: 1, fixedLength:true, unicode: false));

            Sql(@"update CarrierAreas set TransportModeCode = 'A'");

            AlterColumn("dbo.CarrierAreas", "TransportModeCode", c => c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false));
            CreateIndex("dbo.CarrierAreas", "TransportModeCode");
            AddForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes", "Id");

            Sql("update DBIdCounters set TableName = 'CarrierArea' where TableName = 'AirlineArea'");
            Sql("update DBIdCounters set TableName = 'CarrierAreaPort' where TableName = 'AirlineAreaPort'");
        }
        
        public override void Down()
        {
            DropColumn("dbo.CarrierAreas", "TransportModeCode");
            DropForeignKey("dbo.CarrierAreas", "TransportModeCode", "dbo.TransportModes");
            DropIndex("dbo.CarrierAreas", new[] { "TransportModeCode" });
        }
    }
}
