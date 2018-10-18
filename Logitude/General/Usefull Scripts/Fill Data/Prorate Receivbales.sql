
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @MasterShipmentDataId as varchar(15)

set @Tenant = 999999998

if not exists (select * from Tenants where Id = @Tenant)
BEGIN
	print 'Tenant: ' + convert(varchar,@Tenant) + ' does not exists'
END

else
BEGIN
	if ((select ProrateMasterReceivables from Tenants where Id = @Tenant) = 0)
	BEGIN
		update Tenants set ProrateMasterReceivables = 1 where Id = @Tenant
	END

	BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, MasterShipmentDataId
		FROM Shipments
		where Tenant = @Tenant AND ShipmentLevelCode = 'C' AND MasterShipmentDataId is not null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @ShipmentId, @MasterShipmentDataId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			if exists (select * from ShipmentMasterDatas where Id = @MasterShipmentDataId AND Tenant = @Tenant)
			BEGIN
				update ShipmentMasterDatas set ProrateReceivables = 1 where Id = @MasterShipmentDataId AND Tenant = @Tenant
				EXECUTE [usp_UpdateReceivablesData] @ShipmentId
				EXECUTE [usp_UpdateShipmentProfit] @ShipmentId
			END

		FETCH NEXT FROM DataCursor INTO @ShipmentId, @MasterShipmentDataId
		END
		CLOSE DataCursor
		DEALLOCATE DataCursor
	END
END

