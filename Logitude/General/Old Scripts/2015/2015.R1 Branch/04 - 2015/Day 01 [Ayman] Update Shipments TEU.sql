
declare @Tenant as int
declare @ShipmenId as varchar(15)
declare @PackageTypeId as varchar(15)
declare @SumOfTEU as float
declare @PackageTEU as float

BEGIN
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Shipments
	where (TEU is null OR TEU = 0) AND TransportModeId <> 'A' AND ShipmentTypeId <> 'LCL' AND ShipmentTypeId <> 'LTL'
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmenId, @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @SumOfTEU = 0

		if (select count(*) from ShipmentPackages where Tenant = @Tenant AND ShipmentId = @ShipmenId AND PackageTypeId is not null) > 0
		BEGIN
			DECLARE PackagesCursor CURSOR READ_ONLY
			FOR
			SELECT PackageTypeId
			FROM ShipmentPackages
			where Tenant = @Tenant AND ShipmentId = @ShipmenId AND PackageTypeId is not null
			group by PackageTypeId
			OPEN PackagesCursor FETCH NEXT FROM PackagesCursor INTO @PackageTypeId
			WHILE @@FETCH_STATUS = 0
			BEGIN
				
				set @PackageTEU = (select TEU from PackageTypes where Id = @PackageTypeId)
				if (@PackageTEU is not null)
				set @SumOfTEU = @SumOfTEU + @PackageTEU

			FETCH NEXT FROM PackagesCursor INTO @PackageTypeId
			END
			CLOSE PackagesCursor
			DEALLOCATE PackagesCursor
		END
		
		update Shipments set TEU = @SumOfTEU where Id = @ShipmenId AND Tenant = @Tenant

	FETCH NEXT FROM ShipmentsCursor INTO @ShipmenId, @Tenant
	END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END


