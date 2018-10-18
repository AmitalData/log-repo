
declare @Tenant as int
declare @EntityId as varchar(15)

BEGIN -- ARInvoicesCursor
		DECLARE ARInvoicesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant		 
		FROM ARInvoices
		where StatusCode = 'AD' and IsCancelled = 0
		OPEN ARInvoicesCursor FETCH NEXT FROM ARInvoicesCursor INTO @EntityId, @Tenant			 
		WHILE @@FETCH_STATUS = 0
			BEGIN
				
				if exists (select * from TraceEvents 
							where EntityId = @EntityId
							and Tenant = @Tenant
							and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice')
							and EventTypeId = (select Id from EventTypes where Tenant = @Tenant and Code = 'UPIN' and ObjectTableId = (select Id from ObjectTables where Name = 'ARInvoice'))
							)


				update ARInvoices
				set IsClosed = 1,
				StatusCode = 'PD',
				AmountDue = 0,
				AmountDueInLocalCurrency = 0,
				AmountDueInProfitCurrency = 0
				where Id = @EntityId and Tenant = @Tenant

			FETCH NEXT FROM ARInvoicesCursor INTO @EntityId, @Tenant			
			END
		CLOSE ARInvoicesCursor
		DEALLOCATE ARInvoicesCursor

END