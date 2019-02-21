
declare @MasterShipmentDataId as varchar(15)
declare @agentId as varchar(15)
declare @shipmentLevelcode as varchar(1)
declare @Tenant as int
declare @Id as varchar(15)
declare @ConnectedagentId as varchar(15)


begin
	DECLARE ShipmentCursor CURSOR READ_ONLY
	FOR
	SELECT AgentId,MasterShipmentDataId,ShipmentLevelCode,Tenant,Id
	From Shipments
	OPEN ShipmentCursor FETCH NEXT FROM ShipmentCursor INTO @AgentId,@MasterShipmentDataId,@shipmentLevelcode,@Tenant,@Id
	WHILE @@FETCH_STATUS = 0
	BEGIN
		if(@shipmentLevelcode = 'D' OR @shipmentLevelcode='C')
			begin
			update shipments set AgentComputed=@AgentId where Id=@Id and tenant=@Tenant
			Print(@shipmentLevelcode+' ' + @AgentId)
			END
			else if(@shipmentLevelcode = 'H')
			begin
			if(@AgentId != '' AND @AgentId is not null)
			begin
						update shipments set AgentComputed=@AgentId where Id=@Id and tenant=@Tenant
									Print(@shipmentLevelcode+' ' + @AgentId + 'H Exists')

			END
			else if(@MasterShipmentDataId != '' AND @MasterShipmentDataId is not null)
			begin
			 set @ConnectedagentId=( select AgentId from Shipments where Id=@MasterShipmentDataId and tenant=@Tenant)
				update shipments set AgentComputed=@ConnectedagentId where Id=@Id and tenant=@Tenant 
													Print(@shipmentLevelcode+' ' + @AgentId + 'M Exists')
			END
			END
				
		
	FETCH NEXT FROM ShipmentCursor     INTO @AgentId,@MasterShipmentDataId,@shipmentLevelcode,@Tenant,@Id
	END
	CLOSE ShipmentCursor
	DEALLOCATE ShipmentCursor
END

