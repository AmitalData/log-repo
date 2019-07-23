update ShipmentComputedFields 
set Commodity = (select Top 1 CommodityNumber from ShipmentCommodities where ShipmentId = ShipmentComputedFields.Id)

declare @ContainersNumber as varchar(1000)
declare @ContainerNumber as varchar(100)

declare @FirstPickupId varchar(15)
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @TransportMode as varchar(1)
declare @Direction as varchar(1)
declare @Origin as varchar(150)
declare @FirstPickupFromTypeCode as varchar(4)
declare @FirstPickupFromAddressId as varchar(15)
declare @FirstPickupFromPortId as varchar(15)
declare @FirstPickupFromAddressCity as varchar(25)

DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, TransportModeId, DirectionId
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant,@Direction,@TransportMode
	WHILE @@FETCH_STATUS = 0
		BEGIN	

	if(@Direction != 'D' and @TransportMode != 'I')
	begin
set @FirstPickupId = (select top 1 Id from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
				set @FirstPickupFromTypeCode = (select top 1 PickUpDeliveryFromTypeCode from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
set @FirstPickupFromAddressId = (select top 1 FromAddressId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
				set @FirstPickupFromPortId = (select top 1 FromPortId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
				set @FirstPickupFromAddressCity = (select top 1 FromAddressCity from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)

if @FirstPickupId is not null
				begin  
					if @FirstPickupFromTypeCode = 'PART'
					begin
						if @FirstPickupFromAddressId is not null
						begin
							set @Origin = (select City from Addresses where Id = @FirstPickupFromAddressId and Tenant = @Tenant)
						end
					end

					else if @FirstPickupFromTypeCode = 'PORT'
					begin
						if @FirstPickupFromPortId is not null
						begin
							set @Origin = (select EnglishName from Ports where Id = @FirstPickupFromPortId and Tenant = @Tenant)
						end
					end

					else if @FirstPickupFromTypeCode = 'CASL'
					begin
						set @Origin = @FirstPickupFromAddressCity
					end
			
					update ShipmentComputedFields set FirstPickupLocation=@Origin where tenant=@tenant and Id=@ShipmentId
				end
end

		FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @Direction,@TransportMode
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
	

	
	
