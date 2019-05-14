
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @MasterShipmentDataId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @MainCarriageFromPortId as varchar(15)
declare @PreCarriageFromPortId as varchar(15)
declare @FromPortId as varchar(15)
declare @ToPortId as varchar(15)
declare @MainCarriageFromAddressId as varchar(15)
declare @MainCarriageToAddressId as varchar(15)
declare @WarehouseLegWarehouseId as varchar(15)

declare @TransportMode as varchar(1)
declare @Direction as varchar(1)
declare @Level as varchar(1)

declare @FirstPickupId varchar(15)
declare @FirstPickupFromTypeCode as varchar(4)
declare @FirstPickupFromAddressId as varchar(15)
declare @FirstPickupFromPortId as varchar(15)
declare @FirstPickupFromAddressCity as varchar(25)

declare @LastDeliveryId varchar(15)
declare @LastDeliveryToTypeCode as varchar(4)
declare @LastDeliveryToAddressId as varchar(15)
declare @LastDeliveryToPortId as varchar(15)
declare @LastDeliveryToAddressCity as varchar(25)

declare @From as varchar(150)
declare @To as varchar(150)
declare @Origin as varchar(150)
declare @FinalDestination as varchar(150)

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, MasterShipmentDataId, OnCarriageToPortId, PreCarriageFromPortId, TransportModeId, DirectionId, ShipmentLevelCode, FromPortId, ToPortId, WarehouseLegWarehouseId
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @MasterShipmentDataId, @OnCarriageToPortId, @PreCarriageFromPortId, @TransportMode, @Direction, @Level, @FromPortId, @ToPortId, @WarehouseLegWarehouseId
	WHILE @@FETCH_STATUS = 0
		BEGIN
			
			if(@Direction = 'D' and @TransportMode = 'I')
			begin
				set @MainCarriageFromAddressId = (select MainCarriageFromAddressId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
				set @MainCarriageToAddressId = (select MainCarriageToAddressId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)

				set @From = (select City from Addresses where Id = @MainCarriageFromAddressId and Tenant = @Tenant)
				set @Origin = (select City from Addresses where Id = @MainCarriageFromAddressId and Tenant = @Tenant)
				set @To = (select City from Addresses where Id = @MainCarriageToAddressId and Tenant = @Tenant)
				set @FinalDestination = (select City from Addresses where Id = @MainCarriageToAddressId and Tenant = @Tenant)
			end

			else
			begin
				set @MainCarriageFromPortId = (select MainCarriageFromPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
				set @Transshipment3ToPortId = (select Transshipment3ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
			    set @Transshipment2ToPortId = (select Transshipment2ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
				set @Transshipment1ToPortId = (select Transshipment1ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
				set @MainCarriageToPortId = (select MainCarriageToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)

				-------------From-------------
				if(@Level = 'H')
				begin
					set @From = (select EnglishName from [Ports] where Id = @FromPortId and Tenant = @Tenant)
				end

				else
				begin					
					set @From = (select EnglishName from [Ports] where Id = @MainCarriageFromPortId and Tenant = @Tenant)
				end 
				
				-------------To-------------
				if(@Level = 'H')
				begin
					set @To = (select EnglishName from [Ports] where Id = @ToPortId and Tenant = @Tenant)
				end

				else
				begin					
					if @Transshipment3ToPortId is not null
					begin
						set @To = (select EnglishName from Ports where Id = @Transshipment3ToPortId and Tenant = @Tenant)
					end

					else if @Transshipment2ToPortId is not null
					begin
						set @To = (select EnglishName from Ports where Id = @Transshipment2ToPortId and Tenant = @Tenant)
					end

					else if @Transshipment1ToPortId is not null
					begin
						set @To = (select EnglishName from Ports where Id = @Transshipment1ToPortId and Tenant = @Tenant)
					end

					else if @MainCarriageToPortId is not null
					begin
						set @To = (select EnglishName from Ports where Id = @MainCarriageToPortId and Tenant = @Tenant)
					end
				end 

				-------------Origin-------------
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
				end
				
				else if @WarehouseLegWarehouseId is not null and @Direction = 'E'
				begin
					set @Origin = (select EnglishName from Cards where Id = @WarehouseLegWarehouseId and Tenant = @Tenant)
				end

				else if @PreCarriageFromPortId is not null
				begin
					set @Origin = (select EnglishName from Ports where Id = @PreCarriageFromPortId and Tenant = @Tenant)
				end

				else
				begin
					if(@Level = 'H')
					begin
						set @Origin = (select EnglishName from [Ports] where Id = @FromPortId and Tenant = @Tenant)
					end

					else
					begin
						set @Origin = (select EnglishName from Ports where Id = @MainCarriageFromPortId and Tenant = @Tenant)
					end					
				end

				-------------Final Destination-------------
				set @LastDeliveryId = (select top 1 Id from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)
				set @LastDeliveryToTypeCode = (select top 1 PickUpDeliveryToTypeCode from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)
				set @LastDeliveryToAddressId = (select top 1 ToAddressId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)
				set @LastDeliveryToPortId = (select top 1 ToPortId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)
				set @LastDeliveryToAddressCity = (select top 1 ToAddressCity from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)

				if @LastDeliveryId is not null
				begin  
					if @LastDeliveryToTypeCode = 'PART'
					begin
						if @LastDeliveryToAddressId is not null
						begin
							set @FinalDestination = (select City from Addresses where Id = @LastDeliveryToAddressId and Tenant = @Tenant)
						end
					end

					else if @LastDeliveryToTypeCode = 'PORT'
					begin
						if @LastDeliveryToPortId is not null
						begin
							set @FinalDestination = (select EnglishName from Ports where Id = @LastDeliveryToPortId and Tenant = @Tenant)
						end
					end

					else if @LastDeliveryToTypeCode = 'CASL'
					begin
						set @FinalDestination = @LastDeliveryToAddressCity
					end

				end

				else if @WarehouseLegWarehouseId is not null and @Direction = 'I'
				begin
					set @FinalDestination = (select EnglishName from Cards where Id = @WarehouseLegWarehouseId and Tenant = @Tenant)
				end

				else if @OnCarriageToPortId is not null
				begin
					set @FinalDestination = (select EnglishName from Ports where Id = @OnCarriageToPortId and Tenant = @Tenant)
				end

				else 
				begin
					if @Transshipment3ToPortId is not null
					begin
						set @FinalDestination = (select EnglishName from Ports where Id = @Transshipment3ToPortId and Tenant = @Tenant)
					end

					else if @Transshipment2ToPortId is not null
					begin
						set @FinalDestination = (select EnglishName from Ports where Id = @Transshipment2ToPortId and Tenant = @Tenant)
					end

					else if @Transshipment1ToPortId is not null
					begin
						set @FinalDestination = (select EnglishName from Ports where Id = @Transshipment1ToPortId and Tenant = @Tenant)
					end

					else
					begin
						if(@Level = 'H')
						begin
							set @FinalDestination = (select EnglishName from [Ports] where Id = @ToPortId and Tenant = @Tenant)
						end

						else
						begin
							set @FinalDestination = (select EnglishName from Ports where Id = @MainCarriageToPortId and Tenant = @Tenant)
						end					
					end

				end

			end

		    update Shipments set [From] = @From, [To] = @To, Origin = @Origin, LastFinalDestination = @FinalDestination where Id = @ShipmentId

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @MasterShipmentDataId, @OnCarriageToPortId, @PreCarriageFromPortId, @TransportMode, @Direction, @Level, @FromPortId, @ToPortId, @WarehouseLegWarehouseId
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END