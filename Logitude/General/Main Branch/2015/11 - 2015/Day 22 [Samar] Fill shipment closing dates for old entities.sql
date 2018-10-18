declare @OperationalEventTypeId as varchar(15)
declare @AccountingEventTypeId as varchar(15)
declare @ShipmentId as varchar(15)
declare @IsOperationalClosed as bit
declare @IsAccountingClosed as bit
declare @OperationalClosingDate as datetime
declare @AccountingClosingDate as datetime
declare @Tenant as int
declare @TableId as varchar(15)
set @TableId = (select Id from ObjectTables where Name = 'Shipment')

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, IsOperationalClosed, IsAccountingClosed, Tenant
	FROM Shipments
	where IsOperationalClosed = 1 or IsAccountingClosed = 1
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @IsOperationalClosed, @IsAccountingClosed, @Tenant		
	WHILE @@FETCH_STATUS = 0
		BEGIN

			set @OperationalClosingDate = null
			set @AccountingClosingDate = null

			if (@IsOperationalClosed = 1)
			BEGIN
				set @OperationalEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'OPCL' and ObjectTableId = @TableId)
			    set @OperationalClosingDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @ShipmentId and ObjectTableId = @TableId and EventTypeId = @OperationalEventTypeId)										
			END


			if(@IsAccountingClosed = 1)			
			BEGIN
				set @AccountingEventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'ACCL' and ObjectTableId = @TableId)
				set @AccountingClosingDate = (select MAX(LogDateTime) from TraceEvents where Tenant = @Tenant and EntityId = @ShipmentId and ObjectTableId = @TableId and EventTypeId = @AccountingEventTypeId)							
			END

			update Shipments
			set
			OperationalCloseDate = @OperationalClosingDate,
			AccountingCloseDate = @AccountingClosingDate
			where Id = @ShipmentId					

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @IsOperationalClosed, @IsAccountingClosed, @Tenant
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END