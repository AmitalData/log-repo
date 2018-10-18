

declare @Tenant as int
declare @MainEntityId as varchar(15)
declare @CancelledByEntityId as varchar(15)

BEGIN
		DECLARE ARInvoicesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, CancelledByARInvoiceId
		FROM ARInvoices
		where StatusCode = 'AR'
		OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @MainEntityId, @Tenant, @CancelledByEntityId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			update ARInvoices set CreditedByARInvoiceId = @MainEntityId where Tenant = @Tenant and Id = @CancelledByEntityId

		FETCH NEXT FROM ARInvoicesCursor INTO @MainEntityId, @Tenant, @CancelledByEntityId

		END				
		CLOSE ARInvoicesCursor
		DEALLOCATE ARInvoicesCursor
END