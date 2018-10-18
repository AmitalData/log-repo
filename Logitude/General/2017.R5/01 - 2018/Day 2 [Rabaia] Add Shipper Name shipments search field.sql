
declare @Tenant as int
declare @EntityId as varchar(15)
declare @ShipperName as nvarchar(25) 

BEGIN
		DECLARE ShipmentsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ShipperName
		FROM Shipments
		OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @ShipperName
		WHILE @@FETCH_STATUS = 0
		BEGIN
 
			if (@ShipperName is not null)
			begin
				update Shipments set SearchFields = SearchFields + ',' + @ShipperName where Id = @EntityId AND Tenant = @Tenant
			end 
		FETCH NEXT FROM ShipmentsCursor INTO @EntityId, @Tenant, @ShipperName

		END				
		CLOSE ShipmentsCursor
		DEALLOCATE ShipmentsCursor
END

	