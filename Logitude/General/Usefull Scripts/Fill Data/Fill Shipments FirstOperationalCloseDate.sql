
declare @Tenant as int
declare @EntityId as varchar(15)
declare @EventTypeId as varchar(15)
declare @ObjectTableId1 as varchar(15)
declare @ObjectTableId2 as varchar(15)
declare @FirstEventDate as datetime
declare @ShipmentLevelCode as varchar(15)
set @ObjectTableId1 = (select Id from ObjectTables where Name = 'Master')
set @ObjectTableId2 = (select Id from ObjectTables where Name = 'Shipment')
if (@ObjectTableId1 is null) set @ObjectTableId1 = '88888'
if (@ObjectTableId2 is null) set @ObjectTableId2 = '88888'

DECLARE TenantsCursor CURSOR READ_ONLY
FOR
SELECT Id
FROM Tenants
OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
WHILE @@FETCH_STATUS = 0
BEGIN

	set @EventTypeId = (select Id from EventTypes where Code = 'OPCL' AND Tenant = @Tenant AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2))
	if (@EventTypeId is not null)
	begin
		if exists (select * from TraceEvents where Tenant = @Tenant AND EventTypeId = @EventTypeId AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2))
		begin

			DECLARE TraceEventsCursor CURSOR READ_ONLY
			FOR
			SELECT EntityId
			FROM TraceEvents where Tenant = @Tenant AND EventTypeId = @EventTypeId AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2)
			Group by EntityId
			OPEN TraceEventsCursor FETCH NEXT FROM TraceEventsCursor INTO @EntityId
			WHILE @@FETCH_STATUS = 0
			BEGIN

			set @FirstEventDate =
								(select top 1 LogDateTime from TraceEvents 
								where Tenant = @Tenant
								AND EventTypeId = @EventTypeId
								AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2)
								AND EntityId = @EntityId
								order by LogDateTime)

			
			set @ShipmentLevelCode = (select ShipmentLevelCode from Shipments where Tenant = @Tenant AND Id = @EntityId AND ShipmentLevelCode in ('D', 'C') AND FirstOperationalCloseDate is null)
			if (@ShipmentLevelCode is not null)
			begin
				update Shipments set FirstOperationalCloseDate = @FirstEventDate where Tenant = @Tenant AND Id = @EntityId

				if (@ShipmentLevelCode = 'C')
				begin
					update Shipments set FirstOperationalCloseDate = @FirstEventDate where Tenant = @Tenant AND ShipmentLevelCode = 'H' AND MasterShipmentDataId = @EntityId AND FirstOperationalCloseDate is null
				end
			end

			FETCH NEXT FROM TraceEventsCursor INTO @EntityId
			END
			CLOSE TraceEventsCursor
			DEALLOCATE TraceEventsCursor

		end
	end

FETCH NEXT FROM TenantsCursor INTO @Tenant
END
CLOSE TenantsCursor
DEALLOCATE TenantsCursor