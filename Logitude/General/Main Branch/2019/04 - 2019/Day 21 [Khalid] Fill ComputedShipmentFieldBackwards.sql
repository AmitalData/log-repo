update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'D'  

declare @Tenant as int
declare @EntityId as varchar(15)
declare @ComputedShipmentNumber as varchar(1000)
declare @ShipmentNumber as varchar(30)
declare @MasterShipmentId as varchar(15)
declare @ShipmentLevelCode as varchar(15)
declare @ParentShipmentNumber as varchar(30)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant,
	ShipmentNumber,MasterShipmentDataId,ShipmentLevelCode
	FROM Shipments where ShipmentLevelCode In ('C','H')
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId,@ShipmentLevelCode
	WHILE @@FETCH_STATUS = 0
	BEGIN
			if (@MasterShipmentId is null and @ShipmentLevelCode = 'H' )
			begin 
			update shipments set ComputedShipmentNumber=@ShipmentNumber where Tenant=@Tenant and Id=@EntityId
			end

			if (@MasterShipmentId is not null and @ShipmentLevelCode = 'H' )
			begin 
			set @ParentShipmentNumber = (SELECT ShipmentNumber
			FROM Shipments where Tenant=@Tenant and Id=@MasterShipmentId)			
			update shipments set ComputedShipmentNumber=@ParentShipmentNumber where Tenant=@Tenant and Id=@EntityId
			end

						if (@ShipmentLevelCode = 'C' )
						begin
						set @ComputedShipmentNumber = (select ShipmentNumber + ','    from Shipments where MasterShipmentDataId = @EntityId and tenant=@Tenant FOR XML PATH ('') )
						print(@ComputedShipmentNumber + '- ' + @EntityId)
						update shipments set ComputedShipmentNumber=@ComputedShipmentNumber where tenant=@Tenant and Id=@EntityId
						end

						

	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId,@ShipmentLevelCode
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END

