
--update CommunicationLogs
--set TenantName = (select Company from Tenants where Id = Tenant)
--go


declare @TableId as varchar(15)
declare @TableName as varchar(30)
declare @EntityId as varchar(15)
declare @MasterEntityId as varchar(15)
declare @MasterNumber as varchar(20)

BEGIN
	DECLARE LogsCursor CURSOR READ_ONLY
	FOR
	SELECT ObjectTableId, EntityId
	FROM CommunicationLogs
	where EntityId is not null and ObjectTableId is not null and Tenant != 0
	OPEN LogsCursor FETCH NEXT FROM LogsCursor INTO @TableId, @EntityId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @TableName =  (select Name from ObjectTables where Id = @TableId)

		if (@TableName = 'Shipment')
		begin
			set @MasterEntityId =  (select MasterShipmentDataId from Shipments where Id = @EntityId)

			if (@MasterEntityId is not null)
			begin
				set @MasterNumber =  (select [Master] from ShipmentMasterDatas where Id = @MasterEntityId)
			end

		end

		else if (@TableName = 'Booking')
		begin
			set @MasterNumber =  (select [Master] from Bookings where Id = @EntityId)
		end

		update CommunicationLogs
		set AWBNumber = @MasterNumber
		where EntityId = @EntityId and ObjectTableId = @TableId

	FETCH NEXT FROM LogsCursor INTO @TableId, @EntityId
	END
	CLOSE LogsCursor
	DEALLOCATE LogsCursor
END