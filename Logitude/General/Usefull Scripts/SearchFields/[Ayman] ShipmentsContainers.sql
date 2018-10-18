
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @SearchFields as nvarchar(1000)
declare @ContainerNumber as varchar(20)
declare @IsUpdatingShipment as bit

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, SearchFields
	FROM Shipments
	where (TransportModeId = 'O' OR TransportModeId = 'I')
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @SearchFields
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @IsUpdatingShipment = 0

		DECLARE PackagesCursor CURSOR READ_ONLY
		FOR
		SELECT ContainerNumber
		FROM ShipmentPackages
		where ShipmentId = @ShipmentId AND ContainerNumber is not null
		OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if (@SearchFields NOT LIKE '%' + @ContainerNumber + '%')
			begin
				set @IsUpdatingShipment = 1			
				set @SearchFields = @SearchFields + ',' + @ContainerNumber
			end

		FETCH NEXT FROM PackagesCursor INTO @ContainerNumber
		END
		CLOSE PackagesCursor
		DEALLOCATE PackagesCursor

		if (@IsUpdatingShipment = 1)
		begin			
			update Shipments set SearchFields = @SearchFields where Id = @ShipmentId
		end

	FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @SearchFields
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
