
declare @Tenant as int
declare @InvoiceId as varchar(15)
declare @BillToId as varchar(15)
declare @SalesmanId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, BillToId
	FROM ARInvoices
	where BillToId is not null AND SalesmanUserId is null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @InvoiceId, @Tenant, @BillToId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @SalesmanId = (select SalesmanUserId from Customers where Tenant = @Tenant AND Id = @BillToId)
	if (@SalesmanId is not null)
	begin	
		update ARInvoices set SalesmanUserId = @SalesmanId where Id = @InvoiceId AND Tenant = @Tenant
	end

	FETCH NEXT FROM DataCursor INTO @InvoiceId, @Tenant, @BillToId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
