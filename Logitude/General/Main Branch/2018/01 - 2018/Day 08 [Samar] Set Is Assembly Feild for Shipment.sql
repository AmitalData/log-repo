
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @IsAssembly as bit
declare @Count as int

BEGIN
		DECLARE AssembliesCursor CURSOR READ_ONLY
		FOR
		SELECT COUNT(ShipmentId), ShipmentId, Tenant
		FROM ShipmentAssemblies
		GROUP BY ShipmentId, Tenant
		ORDER BY ShipmentId
		OPEN AssembliesCursor FETCH NEXT FROM AssembliesCursor INTO @Count, @ShipmentId, @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if (@Count > 0)
			begin
				set @IsAssembly = 1

				update Shipments set IsAssembly = @IsAssembly where Id = @ShipmentId AND Tenant = @Tenant
			end

		FETCH NEXT FROM AssembliesCursor INTO @Count, @ShipmentId, @Tenant

		END				
		CLOSE AssembliesCursor
		DEALLOCATE AssembliesCursor
END