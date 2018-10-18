
declare @Id as varchar(15)
declare @Tenant as int
declare @MainCarriageCarrierId as varchar(15)
declare @InterlineId as varchar(15)
declare @Prefix as varchar(3)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT ShipmentMasterDatas.Id, ShipmentMasterDatas.Tenant, ShipmentMasterDatas.MainCarriageCarrierId, ShipmentMasterDatas.InterlineId
	FROM ShipmentMasterDatas	
	inner JOIN Shipments on ShipmentMasterDatas.Id = Shipments.Id AND ShipmentMasterDatas.Tenant = Shipments.Tenant
	WHERE Shipments.ShipmentLevelCode in ('D','C') AND Shipments.TransportModeId = 'A'
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @MainCarriageCarrierId, @InterlineId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Prefix = null

	if (@InterlineId is not null)
	begin
		set @Prefix = (select Prefix from Airlines where Id = @InterlineId AND Tenant = @Tenant)
	end

	else if (@MainCarriageCarrierId is not null)
	begin
		set @Prefix = (select Prefix from Airlines where Id = @MainCarriageCarrierId AND Tenant = @Tenant)
	end

	update ShipmentMasterDatas set AirlinePrefix = @Prefix where Id = @Id AND Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO  @Id, @Tenant, @MainCarriageCarrierId, @InterlineId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END