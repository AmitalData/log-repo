

declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @MainEntityId as varchar(15)
declare @MainEntityReference as varchar(15)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, MainEntityId
		FROM APInvoices
		where MainEntityReference is null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @InvoiceId, @Tenant, @MainEntityId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if (@MainEntityId is not null)
			begin
				set @MainEntityReference = (select ShipmentNumber from Shipments where Tenant = @Tenant AND Id = @MainEntityId)

				update APInvoices
				set MainEntityReference = @MainEntityReference
				where Tenant = @Tenant AND Id = @InvoiceId
			end

			FETCH NEXT FROM DataCursor INTO @InvoiceId, @Tenant, @MainEntityId
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END
