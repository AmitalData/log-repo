update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'D'  
update Shipments set ComputedShipmentNumber = ShipmentNumber where ShipmentLevelCode = 'C'  

declare @Tenant as int
declare @EntityId as varchar(15)
declare @MasterShipmentId as varchar(15)
declare @ParentShipmentNumber as varchar(30)
declare @ShipmentNumber as varchar(30)

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant,
	ShipmentNumber,MasterShipmentDataId
	FROM Shipments where ShipmentLevelCode In ('H')
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId
	WHILE @@FETCH_STATUS = 0
	BEGIN
			if (@MasterShipmentId is null )
			begin 
			update shipments set ComputedShipmentNumber=null where Tenant=@Tenant and Id=@EntityId
			end

			if (@MasterShipmentId is not null )
			begin 
			set @ParentShipmentNumber = (SELECT ShipmentNumber
			FROM Shipments where Tenant=@Tenant and Id=@MasterShipmentId)			
			update shipments set ComputedShipmentNumber=@ParentShipmentNumber where Tenant=@Tenant and Id=@EntityId
			end
											
	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @ShipmentNumber,@MasterShipmentId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END

