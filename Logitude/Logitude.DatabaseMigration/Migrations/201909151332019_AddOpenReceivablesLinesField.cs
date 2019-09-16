namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOpenReceivablesLinesField : DbMigration
    {
        public override void Up()
        {            
            AddColumn("dbo.Shipments", "OpenReceivablesLines", c => c.Int(nullable: false));

            Sql(
                @"
	            declare @ShipmentId as varchar(15)
	            declare @OpenReceivablesLines as int

	            DECLARE ShipmentsCursor CURSOR READ_ONLY
	            FOR
	            SELECT ShipmentId, count(*)
	            FROM ShipmentReceivables
	            WHERE ShipmentReceivableLineStatusCode <> 'ACCT'
	            group by ShipmentId
	            OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @OpenReceivablesLines
	            WHILE @@FETCH_STATUS = 0
	            BEGIN

		            update Shipments set OpenReceivablesLines = @OpenReceivablesLines where Id = @ShipmentId

	            FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @OpenReceivablesLines
	            END
	            CLOSE ShipmentsCursor
	            DEALLOCATE ShipmentsCursor
                ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "OpenReceivablesLines");
        }
    }
}
