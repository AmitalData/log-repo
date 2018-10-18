
declare @CustomerId as varchar(15)
declare @AccountManagerUserId as varchar(15)

BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT CustomerId
	FROM Shipments
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @CustomerId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @AccountManagerUserId =  (select AccountManagerUserId from Customers where Id = @CustomerId)

		update Shipments
		set AccountManagerUserId = @AccountManagerUserId
		where CustomerId = @CustomerId

	FETCH NEXT FROM ShipmentsCursor INTO @CustomerId
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END