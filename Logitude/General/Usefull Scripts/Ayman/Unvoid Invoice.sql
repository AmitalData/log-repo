
DECLARE @Tenant AS INT 
DECLARE @InvoiceId AS varchar(15)
DECLARE @InvoiceLineId AS varchar(15)
declare @ChargesTypeId as varchar(15)
declare @ReceivableId as varchar(15)
declare @InvoiceNumber as varchar(25)
DECLARE @ShipmentId AS varchar(15)
DECLARE @ShipmentNumber AS varchar(25)
DECLARE @ObjectTableId AS varchar(15)
DECLARE @ShipmentLevelCode AS varchar(1)
DECLARE @NewId AS varchar(15)

set @Tenant = 327
set @InvoiceNumber = '5523'

BEGIN
	DECLARE InvoicesCursor CURSOR READ_ONLY
	FOR	
	SELECT Id,MainEntityId, MainEntityReference
	FROM ARInvoices
	where Tenant = @Tenant AND StatusCode = 'VD' AND InvoiceNumber = @InvoiceNumber
	OPEN InvoicesCursor FETCH NEXT FROM InvoicesCursor INTO @InvoiceId,@ShipmentId,@ShipmentNumber
	WHILE @@FETCH_STATUS = 0
	BEGIN

	if not exists (select * from ARInvoiceEntities where Tenant = @Tenant AND ARInvoiceId = @InvoiceId AND EntityId = @ShipmentId)
	begin
		set @ShipmentLevelCode = (select ShipmentLevelCode from Shipments where Id = @ShipmentId AND Tenant = @Tenant)
		if (@ShipmentLevelCode = 'C')
		set @ObjectTableId = (select Id from ObjectTables where Name = 'Master')
		else set @ObjectTableId = (select Id from ObjectTables where Name = 'Shipment')

		EXECUTE usp_GetNextTableIdValue @NewId OUTPUT,'ARInvoiceEntity'
		insert into ARInvoiceEntities(Id, Tenant, EntityId, EntityReference, ARInvoiceId, ObjectTableId)
		values (@NewId, @Tenant, @ShipmentId, @ShipmentNumber, @InvoiceId, @ObjectTableId)
	end

		BEGIN
		DECLARE LinesCursor CURSOR READ_ONLY
		FOR	
		SELECT Id,ChargesTypeId
		FROM ARInvoiceLines
		where Tenant = @Tenant AND ARInvoiceId = @InvoiceId
		OPEN LinesCursor FETCH NEXT FROM LinesCursor INTO @InvoiceLineId,@ChargesTypeId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @ReceivableId = (select top 1 Id from ShipmentReceivables where Tenant = @Tenant AND ShipmentId = @ShipmentId AND ChargesTypeId = @ChargesTypeId AND ShipmentReceivableLineStatusCode = 'OAMT' AND ARInvoiceId is null and ARInvoiceLineId is null)

			if (@ReceivableId is not null)
			begin
				
				update ShipmentReceivables
				set ShipmentReceivableLineStatusCode = 'ACCT',
				ARInvoiceId = @InvoiceId,
				ARInvoiceLineId = @InvoiceLineId
				where Id = @ReceivableId

				update ARInvoiceLines set ReceivableId = @ReceivableId where Id = @InvoiceLineId
			end

		FETCH NEXT FROM LinesCursor INTO @InvoiceLineId,@ChargesTypeId
		END
		CLOSE LinesCursor
		DEALLOCATE LinesCursor
		END

		update ARInvoices set StatusCode = 'AD' where Id = @InvoiceId

		EXECUTE usp_UpdateShipmentProfitFunction @Tenant, @ShipmentId, 0

	FETCH NEXT FROM InvoicesCursor INTO @InvoiceId,@ShipmentId,@ShipmentNumber
	END
	CLOSE InvoicesCursor
	DEALLOCATE InvoicesCursor
END