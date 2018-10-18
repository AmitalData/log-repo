
DECLARE @Tenant AS INT
DECLARE @ShipmentId AS varchar(15)
DECLARE @CarrierNumber AS varchar(15)
DECLARE @VesselId AS varchar(15)
DECLARE @SearchFields AS nvarchar(1000)

BEGIN;
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR	
	SELECT Tenant,Id,SearchFields
	FROM Shipments 
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @Tenant,@ShipmentId,@SearchFields
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @CarrierNumber = (select MainCarriageCarrierNumber from ShipmentMasterDatas where Id = @ShipmentId AND Tenant = @Tenant)
		set @VesselId = (select MainCarriageVesselId from ShipmentMasterDatas where Id = @ShipmentId AND Tenant = @Tenant)

		if(@CarrierNumber is not null)
			BEGIN;
				set @SearchFields = @SearchFields + ',' + @CarrierNumber
			END

		if(@VesselId is not null)
			BEGIN;
				set @SearchFields = @SearchFields + ',' + (select EnglishName from Vessels where Id = @VesselId AND Tenant = @Tenant)
			END

		update Shipments set SearchFields = @SearchFields where Id = @ShipmentId AND Tenant = @Tenant

	FETCH NEXT FROM ShipmentsCursor INTO @Tenant,@ShipmentId,@SearchFields
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END