

declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @NumberOfHouses as int

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Shipments
	where ShipmentLevelCode = 'C'
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @NumberOfHouses = (select count(*) from Shipments where ShipmentLevelCode = 'H' ANd MasterShipmentDataId = @ShipmentId AND Tenant = @Tenant)
		
		update ShipmentComputedFields
		set NumberOfHouses = @NumberOfHouses
		where Tenant = @Tenant AND Id = @ShipmentId


	FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END