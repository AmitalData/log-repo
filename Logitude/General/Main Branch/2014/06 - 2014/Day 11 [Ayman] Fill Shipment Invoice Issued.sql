
update Shipments set ARInvoiceIssued = 0, CreditNoteIssued = 0
go

declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @ShipmentId as varchar(15)

	DECLARE ARInvoiceEntityCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant, ARInvoiceId, EntityId
	FROM ARInvoiceEntities
	GROUP by Tenant, ARInvoiceId, EntityId
	OPEN ARInvoiceEntityCursor FETCH NEXT FROM ARInvoiceEntityCursor INTO @Tenant, @InvoiceId, @ShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		declare @IsInvoiceCancelled as bit
		set @IsInvoiceCancelled = (select IsCancelled from ARInvoices where Id = @InvoiceId and Tenant = @Tenant)
		if(@IsInvoiceCancelled = 0)
		begin

			declare @InvoiceTypeCode as varchar(3)
			set @InvoiceTypeCode = (select ARInvoiceTypeCode from ARInvoices where Id = @InvoiceId and Tenant = @Tenant)

			if(@InvoiceTypeCode = 'CD')
			update Shipments set CreditNoteIssued = 1 where Id = @ShipmentId And Tenant = @Tenant

			else
			update Shipments set ARInvoiceIssued = 1 where Id = @ShipmentId And Tenant = @Tenant
		end	

	FETCH NEXT FROM ARInvoiceEntityCursor INTO @Tenant, @InvoiceId, @ShipmentId
	END
	CLOSE ARInvoiceEntityCursor
	DEALLOCATE ARInvoiceEntityCursor

	-- All Shipments Query
	--select * from Shipments where ARInvoiceIssued = 1 and IsCancelled = 0 and Tenant = 1 AND IsOperationalClosed = 0 and (ShipmentLevelCode = 'D' Or ShipmentLevelCode = 'H')
	--go