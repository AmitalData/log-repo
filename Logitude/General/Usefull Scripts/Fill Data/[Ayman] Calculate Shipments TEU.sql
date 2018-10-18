

declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @PackageTypeId as varchar(15)
declare @ShipmentTEU as float(15)

declare @TEU as float
declare @_teu as float

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, TEU
	FROM Shipments
	where TransportModeId != 'A'
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @ShipmentTEU
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @TEU = null

		-- Start Loop Packages
		DECLARE DataInnerCursor CURSOR READ_ONLY
		FOR
		SELECT PackageTypeId
		FROM ShipmentPackages 
		where ShipmentId = @ShipmentId AND Tenant = @Tenant AND PackageTypeId is not null
		OPEN DataInnerCursor FETCH NEXT FROM DataInnerCursor INTO @PackageTypeId
		WHILE @@FETCH_STATUS = 0
		BEGIN
			
			if(@TEU is null)
			set @TEU = 0

			set @_teu = (select TEU from PackageTypes where Tenant = @Tenant AND Id = @PackageTypeId)
			if(@_teu is not null)
			set @TEU = @TEU + @_teu
		
		FETCH NEXT FROM DataInnerCursor INTO @PackageTypeId
		END
		CLOSE DataInnerCursor
		DEALLOCATE DataInnerCursor		
		-- End loop packages

		if(isnull(@ShipmentTEU,0) != isnull(@TEU,0))
		begin
			update Shipments set TEU = @TEU where Id = @ShipmentId AND Tenant = @Tenant
		end

	FETCH NEXT FROM DataCursor INTO @ShipmentId, @Tenant, @ShipmentTEU
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END