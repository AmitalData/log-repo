
declare @Tenant as int
declare @PaymentId as varchar(15)
declare @InvoiceId as varchar(15)
declare @ShipmentId as varchar(15)
declare @InvoiceNumber as varchar(20)
declare @ShipmentNumber as varchar(15)
declare @Count as int

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT ARPaymentId, Tenant, count(*)
	FROM ARInvoicePayments
	group by ARPaymentId, Tenant	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @PaymentId, @Tenant, @Count
	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		if (@Count = 1)
		begin
			set @InvoiceId = (select top 1 ARInvoiceId from ARInvoicePayments where Tenant = @Tenant AND ARPaymentId = @PaymentId)
			
			select
			@InvoiceNumber = InvoiceNumber,
			@ShipmentId = MainEntityId,
			@ShipmentNumber = MainEntityReference
			from ARInvoices
			where Id = @InvoiceId AND Tenant = @Tenant

			if (@ShipmentNumber is null OR @ShipmentNumber = '')
			begin
				set @ShipmentNumber = (select top 1 ShipmentNumber from Shipments where Tenant = @Tenant AND Id = @ShipmentId)
			end
		end

		else
		begin
			set @InvoiceNumber = 'Multi'
			set @ShipmentNumber = 'Multi'
		end
		
		update ARPayments
		set
		InvoiceNumber = @InvoiceNumber,
		ShipmentNumber = @ShipmentNumber
		where Id = @PaymentId AND Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO @PaymentId, @Tenant, @Count
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END