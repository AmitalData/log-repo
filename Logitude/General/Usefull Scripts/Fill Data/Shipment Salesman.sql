
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @CustomerId as varchar(15)
declare @SalesmanId as varchar(15)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, CustomerId
	FROM Shipments
	where CustomerId is not null and SalesmanUserId is null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @CustomerId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @SalesmanId = (select SalesmanUserId from Customers where Tenant = @Tenant AND Id = @CustomerId)
	if (@SalesmanId is not null)
	begin
		update Shipments set SalesmanUserId = @SalesmanId where Id = @ShipmentId AND Tenant = @Tenant
	end

	FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @CustomerId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END