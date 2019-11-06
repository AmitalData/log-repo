namespace Logitude.DatabaseMigration.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNotInvoicedReceivablesAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shipments", "NotInvoicedReceivablesAmount", c => c.Double());

            Sql(
                @"
	            declare @ShipmentId as varchar(15)
	            declare @NotInvoicedReceivablesAmount as float

	            DECLARE ShipmentsCursor CURSOR READ_ONLY
	            FOR
	            SELECT ShipmentId, sum(isnull(TotalAmountLocal,0))
	            FROM ShipmentReceivables
	            WHERE ShipmentReceivableLineStatusCode = 'OAMT'
	            group by ShipmentId
	            OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @NotInvoicedReceivablesAmount
	            WHILE @@FETCH_STATUS = 0
	            BEGIN

		            update Shipments set NotInvoicedReceivablesAmount = @NotInvoicedReceivablesAmount where Id = @ShipmentId

	            FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @NotInvoicedReceivablesAmount
	            END
	            CLOSE ShipmentsCursor
	            DEALLOCATE ShipmentsCursor
                ");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shipments", "NotInvoicedReceivablesAmount");
        }
    }
}
