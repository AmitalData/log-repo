
declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @MasterShipmentDataId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)

declare @FinalDestination as varchar(150)
declare @ETA as datetime
declare @ETD as datetime

declare @FirstDeliveryId varchar(15)
declare @FirstDeliveryToTypeCode as varchar(4)
declare @FirstDeliveryToAddressId as varchar(15)
declare @FirstDeliveryToPortId as varchar(15)
declare @FirstDeliveryToAddressCity as varchar(25)

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, MasterShipmentDataId, OnCarriageToPortId
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @MasterShipmentDataId, @OnCarriageToPortId
	WHILE @@FETCH_STATUS = 0
		BEGIN

			set @Transshipment3ToPortId = (select Transshipment3ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
			set @Transshipment2ToPortId = (select Transshipment2ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
			set @Transshipment1ToPortId = (select Transshipment1ToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)
			set @MainCarriageToPortId = (select MainCarriageToPortId from ShipmentMasterDatas where Id = @MasterShipmentDataId and Tenant = @Tenant)

			set @FirstDeliveryId = (select top 1 Id from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber)
			set @FirstDeliveryToTypeCode = (select top 1 PickUpDeliveryToTypeCode from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber)
			set @FirstDeliveryToAddressId = (select top 1 ToAddressId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber)
			set @FirstDeliveryToPortId = (select top 1 ToPortId from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber)
			set @FirstDeliveryToAddressCity = (select top 1 ToAddressCity from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber)

			set @ETA = (select top 1 ETA from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
			set @ETD = (select top 1 ETD from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)

		if @FirstDeliveryId is not null
		begin  
			if @FirstDeliveryToTypeCode = 'PART'
			begin
				if @FirstDeliveryToAddressId is not null
				begin
					set @FinalDestination = (select City from Addresses where Id = @FirstDeliveryToAddressId and Tenant = @Tenant)
				end
			end

			else if @FirstDeliveryToTypeCode = 'PORT'
			begin
				if @FirstDeliveryToPortId is not null
				begin
					set @FinalDestination = (select EnglishName from Ports where Id = @FirstDeliveryToPortId and Tenant = @Tenant)
				end
			end

			else if @FirstDeliveryToTypeCode = 'CASL'
			begin
				set @FinalDestination = @FirstDeliveryToAddressCity
			end

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

			else if @MainCarriageToPortId is not null
			begin
				set @FinalDestination = (select EnglishName from Ports where Id = @MainCarriageToPortId and Tenant = @Tenant)
			end

		end

		--print @FinalDestination
		--print @ETA
		--print @ETD
		update Shipments set LastFinalDestination = @FinalDestination, FirstPickupETA = @ETA, FirstPickupETD = @ETD where Id = @ShipmentId

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @MasterShipmentDataId, @OnCarriageToPortId
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END


