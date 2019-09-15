

--select * from ARInvoiceEntities
--join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
--where ARInvoices.IsConstituentInvoice = 1

	-- Loop Houses
	declare @Tenant as int
	declare @InvoiceId as varchar(15)
	declare @ShipmentId as varchar(15)
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT ARInvoiceEntities.ARInvoiceId, ARInvoiceEntities.EntityId, ARInvoiceEntities.Tenant
	FROM ARInvoiceEntities
	join ARInvoices on ARInvoiceEntities.ARInvoiceId = ARInvoices.Id
	where ARInvoices.IsConstituentInvoice = 1 and (ARInvoices.StatusCode = 'NT' OR ARInvoices.StatusCode = 'CN')
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @InvoiceId, @ShipmentId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update ShipmentReceivables set ShipmentReceivableLineStatusCode = 'ACCT'
		where ShipmentId = @ShipmentId
		AND ARInvoiceId = @InvoiceId
		AND Tenant = @Tenant

		EXECUTE usp_UpdateShipmentProfit @ShipmentId


	FETCH NEXT FROM ShipmentsCursor INTO @InvoiceId, @ShipmentId,@Tenant
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor