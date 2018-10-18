


declare @Tenant as int
declare @PRVL_Id as varchar(15)
declare @PRFR_Id as varchar(15)	
declare @ShipmentId as varchar(15)	
declare @ReceivableId as varchar(15)	
declare @InvoiceLineId as varchar(15)	
declare @ReceivableAmount as float	
declare @InvoiceLineAmount as float	

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @PRVL_Id = (select Id from Measurements where Code = 'PRVL' AND Tenant = @Tenant)
		set @PRFR_Id = (select Id from Measurements where Code = 'PRFR' AND Tenant = @Tenant)

		DECLARE ReceivablesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, ARInvoiceLineId, TotalAmount, ShipmentId
		FROM ShipmentReceivables
		where Tenant = @Tenant AND ARInvoiceLineId is not null and MeasurementId is not null
		and MeasurementId in (@PRVL_Id, @PRFR_Id)
		OPEN ReceivablesCursor FETCH NEXT FROM ReceivablesCursor INTO @ReceivableId, @InvoiceLineId, @ReceivableAmount, @ShipmentId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @InvoiceLineAmount = (select ForiegnCurrencyAmount from ARInvoiceLines where Tenant = @Tenant AND Id = @InvoiceLineId)
			if (@InvoiceLineAmount != @ReceivableAmount)
			begin
				--print 'Shipment:' + @ShipmentId + ' / (Tenant:' + convert(varchar,@Tenant) + ')'
				print 'Shipment:' + @ShipmentId + ' / Receivable:' + @ReceivableId + ' / (Tenant:' + convert(varchar,@Tenant) + ')'
			end

		FETCH NEXT FROM ReceivablesCursor INTO @ReceivableId, @InvoiceLineId, @ReceivableAmount, @ShipmentId
		END
		CLOSE ReceivablesCursor
		DEALLOCATE ReceivablesCursor

	FETCH NEXT FROM TenantsCursor INTO @Tenant
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor