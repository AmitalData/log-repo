
begin
declare @ShipmentId as varchar(15)
declare @Id as varchar(15)

declare @Tenant as int

	 DECLARE WarehouseEntryCursor CURSOR READ_ONLY
	FOR
	SELECT Id,ShipmentId,Tenant
	From WarehouseEntries
	 OPEN WarehouseEntryCursor FETCH NEXT FROM WarehouseEntryCursor INTO @Id, @ShipmentId, @Tenant
	 WHILE @@FETCH_STATUS = 0
	BEGIN

	if(@ShipmentId is not null)
	begin
	
	update WarehouseEntries set ConnectedToShipment = 1 where Id = @Id and Tenant = @Tenant
	end

	FETCH NEXT FROM WarehouseEntryCursor INTO   @Id, @ShipmentId, @Tenant
	END
	 CLOSE WarehouseEntryCursor
	 DEALLOCATE WarehouseEntryCursor
END

