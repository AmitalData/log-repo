
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @FHLStatusCode as varchar(4)
declare @FWBStatusCode as varchar(4)
declare @ShipmentLevelCode as varchar(1)

declare @UserId as varchar(15)

declare @TableId as varchar(15)
set @TableId = (select Id from ObjectTables where Name = 'Shipment')


BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Shipments.Id, Shipments.Tenant, Shipments.ShipmentLevelCode, Shipments.FHLStatusCode, ShipmentMasterDatas.FWBStatusCode
	FROM Shipments
	join ShipmentMasterDatas on Shipments.MasterShipmentDataId = ShipmentMasterDatas.Id
	where Shipments.TransportModeId = 'A' and Shipments.MasterShipmentDataId is not null
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ShipmentLevelCode, @FHLStatusCode, @FWBStatusCode
	WHILE @@FETCH_STATUS = 0
		BEGIN


			if (@ShipmentLevelCode = 'H')
			begin
				if (@FHLStatusCode = 'SENT')
				begin
					set @UserId = (select Top 1 CreatedByUserId from CommunicationLogs where Tenant = @Tenant and [Subject] = 'FHL' and EntityId = @ShipmentId and ObjectTableId = @TableId order by CreateDate desc)
					update Shipments set LastSentByUserId = @UserId where Id = @ShipmentId	
				end
			end

			else
			begin
				if (@FWBStatusCode = 'SENT')			 
				 begin
					set @UserId = (select Top 1 CreatedByUserId from CommunicationLogs where Tenant = @Tenant and [Subject] = 'FWB' and EntityId = @ShipmentId and ObjectTableId = @TableId order by CreateDate desc)
					update Shipments set LastSentByUserId = @UserId where Id = @ShipmentId	
				end
			end

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ShipmentLevelCode, @FHLStatusCode, @FWBStatusCode
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END


