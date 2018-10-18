namespace Logitude.SystemLogs.Migrations
{
    using Logitude.SystemLogs.SQL;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropOldFlightsSchedules : DbMigration
    {
        public override void Up()
        {
           // string key = GetForeignKeyName.Execute("FlightsSchedulesAnswers", "FlightsSchedulesRequests", "RequestId", 0);
            //DropForeignKey("dbo.FlightsSchedulesAnswers", key);
            //DropForeignKey("dbo.FlightsSchedulesAnswers", "RequestId", "dbo.FlightsSchedulesRequests");

            //DropIndex("dbo.FlightsSchedulesAnswers", new[] { "RequestId" });
            DropTable("dbo.FlightsSchedulesAnswers");
            DropTable("dbo.FlightsSchedulesRequests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.FlightsSchedulesRequests",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        FromPortCode = c.String(nullable: false, maxLength: 3, unicode: false),
                        ToPortCode = c.String(nullable: false, maxLength: 3, unicode: false),
                        AirlineCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        FlightNumber = c.String(maxLength: 15, unicode: false),
                        ETD = c.DateTime(),
                        ETA = c.DateTime(),
                        HasResponse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FlightsSchedulesAnswers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 15, unicode: false),
                        RequestId = c.String(nullable: false, maxLength: 15, unicode: false),
                        FlightNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        AircraftTypeCode = c.String(nullable: false, maxLength: 5, unicode: false),
                        NumberOfStops = c.Int(nullable: false),
                        ETD = c.DateTime(),
                        ETA = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.FlightsSchedulesAnswers", "RequestId");
            AddForeignKey("dbo.FlightsSchedulesAnswers", "RequestId", "dbo.FlightsSchedulesRequests", "Id", cascadeDelete: true);
        }
    }
}
