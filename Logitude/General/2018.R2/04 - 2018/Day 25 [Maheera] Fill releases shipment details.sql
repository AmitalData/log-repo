begin
declare @ShipmentId as varchar(15)
declare @Id as varchar(15)
declare @DirectionId as varchar(1)
declare @TransportModeId as varchar(1)
declare @DirectionId_updated as varchar(1)
declare @TransportModeId_updated as varchar(1)
declare @Tenant as int

	DECLARE WarehouseReleaseCursor CURSOR READ_ONLY
		FOR
		SELECT Id,ShipmentId,Tenant, DirectionId, TransportModeId
		From WarehouseReleases
		where DirectionId is null or TransportModeId is null
		OPEN WarehouseReleaseCursor FETCH NEXT FROM WarehouseReleaseCursor INTO @Id, @ShipmentId, @Tenant, @DirectionId, @TransportModeId 
		WHILE @@FETCH_STATUS = 0
		BEGIN
		set @DirectionId_updated = null
		set @TransportModeId_updated = null 

		if (@DirectionId is null)
			begin
				set @DirectionId_updated = (select DirectionId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
				Update  WarehouseReleases set DirectionId = @DirectionId_updated where Id = @Id and tenant =@Tenant
			end 

	    if (@TransportModeId is null)
			begin
				set @TransportModeId_updated = (select TransportModeId from Shipments where  Id = @ShipmentId and tenant =@Tenant )
				Update  WarehouseReleases set TransportModeId = @TransportModeId_updated where Id = @Id and tenant =@Tenant
			end 

	FETCH NEXT FROM WarehouseReleaseCursor INTO  @Id, @ShipmentId, @Tenant, @DirectionId, @TransportModeId 
	END
	CLOSE WarehouseReleaseCursor
	DEALLOCATE WarehouseReleaseCursor
END

