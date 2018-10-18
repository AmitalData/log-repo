
declare @Tenant as int
declare @HouseId as varchar(15)
declare @MasterId as varchar(15)
declare @FlightPrefix as varchar(5)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, MasterShipmentDataId
	FROM Shipments
	where ShipmentLevelCode = 'H' and MasterShipmentDataId is not null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @HouseId, @Tenant, @MasterId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @FlightPrefix = (select MainCarriageCarrierPrefix from Shipments where Id = @MasterId and Tenant = @Tenant)
		update Shipments set MainCarriageCarrierPrefix = @FlightPrefix where Id = @HouseId and Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO @HouseId, @Tenant, @MasterId	
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END