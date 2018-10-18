

declare @Tenant as int
declare @CustomerId as varchar(15)

	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Tenant, CustomerId
	From Shipments 
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @Tenant, @CustomerId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update Customers set IsCustomer = 1 where Tenant = @Tenant AND Id = @CustomerId

	FETCH NEXT FROM ShipmentsCursor INTO @Tenant, @CustomerId
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor