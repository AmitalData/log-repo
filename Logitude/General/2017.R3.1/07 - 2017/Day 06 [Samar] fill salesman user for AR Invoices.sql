
declare @Tenant as int
declare @Id as varchar(15)
declare @BillToId as varchar(15)
declare @SalesmanUserId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, BillToId
	FROM ARInvoices
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @BillToId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @SalesmanUserId = null

		set @SalesmanUserId = (select SalesmanUserId from Cards where Id = @BillToId and Tenant = @Tenant)

		update ARInvoices set SalesmanUserId = @SalesmanUserId where Id = @Id AND Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @BillToId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END