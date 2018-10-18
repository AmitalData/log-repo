
declare @ShipmentId as varchar(15)
declare @Tenant as int
declare @ShipmentLevelCode as varchar(1)
declare @MasterShipmentDataId as varchar(15)

--Delivery
declare @ETA as datetime
declare @ATA as datetime

--OnCarriage
declare @OnCarriageFromPortId as varchar(15)
declare @OnCarriageToPortId as varchar(15)
declare @OnCarriageETA as datetime
declare @OnCarriageATA as datetime

--Transshipment1
declare @Transshipment1FromPortId as varchar(15)
declare @Transshipment1ToPortId as varchar(15)
declare @Transshipment1ETA as datetime
declare @Transshipment1ATA as datetime

--Transshipment2
declare @Transshipment2FromPortId as varchar(15)
declare @Transshipment2ToPortId as varchar(15)
declare @Transshipment2ETA as datetime
declare @Transshipment2ATA as datetime

--Transshipment3
declare @Transshipment3FromPortId as varchar(15)
declare @Transshipment3ToPortId as varchar(15)
declare @Transshipment3ETA as datetime
declare @Transshipment3ATA as datetime

--MainCarriage (Inland demostic got no Ports)
declare @MainCarriageFromPortId as varchar(15)
declare @MainCarriageToPortId as varchar(15)
declare @MainCarriageETA as datetime
declare @MainCarriageATA as datetime

declare @EstimatedDeliveriesDate as datetime
declare @ActualDeliveriesDate as datetime

declare @EstimatedFinalArrivalDate as datetime
declare @ActualFinalArrivalDate as datetime

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ShipmentLevelCode,  MasterShipmentDataId, OnCarriageFromPortId, OnCarriageToPortId, OnCarriageETA, OnCarriageATA
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ShipmentLevelCode, @MasterShipmentDataId, @OnCarriageFromPortId, @OnCarriageToPortId, @OnCarriageETA, @OnCarriageATA
	WHILE @@FETCH_STATUS = 0
		BEGIN
					set @EstimatedDeliveriesDate = null
					set @ActualDeliveriesDate = null
			set @EstimatedFinalArrivalDate = null
			set @ActualFinalArrivalDate = null

			if exists (select * from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and Tenant = @Tenant and PickUpDeliveryTypeCode = 'DELV')
			BEGIN
				DECLARE DeliveriesCursor CURSOR READ_ONLY
				FOR
				SELECT ETA, ATA
				FROM ShipmentPickUpDeliveries
				WHERE ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV'

				OPEN DeliveriesCursor FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
				WHILE @@FETCH_STATUS = 0
				BEGIN

					set @EstimatedDeliveriesDate = null
					set @ActualDeliveriesDate = null

					if (@ETA is not null)
					begin
						if (@EstimatedDeliveriesDate is null)
							set @EstimatedDeliveriesDate = @ETA

						else if (@ETA > @EstimatedDeliveriesDate)
							set @EstimatedDeliveriesDate = @ETA 
					end

					if (@ATA is not null)
					begin
						if (@ActualDeliveriesDate is null)
							set @ActualDeliveriesDate = @ATA

						else if (@ATA > @ActualDeliveriesDate)
							set @ActualDeliveriesDate = @ATA 
					end

				FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
				END
				CLOSE DeliveriesCursor
				DEALLOCATE DeliveriesCursor
			END

			if (@EstimatedDeliveriesDate is not null)
				set @EstimatedFinalArrivalDate = @EstimatedDeliveriesDate

			else if (@OnCarriageETA is not null)
				set @EstimatedFinalArrivalDate = @OnCarriageETA

			if (@ActualDeliveriesDate is not null)
				set @ActualFinalArrivalDate = @ActualDeliveriesDate
	
			else if (@OnCarriageATA is not null)
				set @ActualFinalArrivalDate = @OnCarriageATA

			if (@ShipmentLevelCode = 'H')
			BEGIN		
				if(@MasterShipmentDataId is not null)
				begin					
					set @EstimatedFinalArrivalDate = (select EstimatedFinalArrivalDate from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant)
					set @ActualFinalArrivalDate = (select ActualFinalArrivalDate from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant)
				end
			END

			else if (@EstimatedFinalArrivalDate is null and @ActualFinalArrivalDate is null)
			BEGIN

			SELECT		
				@MainCarriageFromPortId = MainCarriageFromPortId,
				@Transshipment1FromPortId = Transshipment1FromPortId,
				@Transshipment2FromPortId = Transshipment2FromPortId,
				@Transshipment3FromPortId = Transshipment3FromPortId,
				@MainCarriageToPortId = MainCarriageToPortId,
				@Transshipment1ToPortId = Transshipment1ToPortId,
				@Transshipment2ToPortId = Transshipment2ToPortId,
				@Transshipment3ToPortId = Transshipment3ToPortId,	
				@Transshipment1ETA = Transshipment1ETA,
				@Transshipment2ETA = Transshipment2ETA,
				@Transshipment3ETA = Transshipment3ETA,
				@MainCarriageETA = MainCarriageETA,
				@Transshipment1ATA = Transshipment1ATA,
				@Transshipment2ATA = Transshipment2ATA,
				@Transshipment3ATA = Transshipment3ATA,
				@MainCarriageATA = MainCarriageATA
				from ShipmentMasterDatas where Id = @ShipmentId

				-- @Transshipment3
				if (@Transshipment3FromPortId is not null and @Transshipment3ToPortId is not null)
				BEGIN
		
					if (@Transshipment3ATA is not null)
					set @ActualFinalArrivalDate = @Transshipment3ATA

					if (@Transshipment3ETA is not null)
					set @EstimatedFinalArrivalDate = @Transshipment3ETA	
				END

				-- @Transshipment2
				if (@Transshipment2FromPortId is not null and @Transshipment2ToPortId is not null)
				BEGIN
		
					if (@Transshipment2ATA is not null)
					set @ActualFinalArrivalDate = @Transshipment2ATA

					if (@Transshipment2ETA is not null)
					set @EstimatedFinalArrivalDate = @Transshipment2ETA	
				END

				-- @Transshipment1
				if (@Transshipment1FromPortId is not null and @Transshipment1ToPortId is not null)
				BEGIN
		
					if (@Transshipment1ATA is not null)
					set @ActualFinalArrivalDate = @Transshipment1ATA

					if (@Transshipment1ETA is not null)
					set @EstimatedFinalArrivalDate = @Transshipment1ETA	
				END

				-- @MainCarriage
				else
				BEGIN
		
					if (@MainCarriageATA is not null)
					set @ActualFinalArrivalDate = @MainCarriageATA

					if (@MainCarriageETA is not null)
					set @EstimatedFinalArrivalDate = @MainCarriageETA	
				END				

			END

			update Shipments set EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate, ActualFinalArrivalDate = @ActualFinalArrivalDate 
				where Id = @ShipmentId and Tenant = @Tenant

				if (@ShipmentLevelCode = 'C')
				begin	

					update Shipments set EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate, ActualFinalArrivalDate = @ActualFinalArrivalDate
					where MasterShipmentDataId = @ShipmentId and Tenant = @Tenant and ShipmentLevelCode = 'H'
				end

			FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant, @ShipmentLevelCode, @MasterShipmentDataId, @OnCarriageFromPortId, @OnCarriageToPortId, @OnCarriageETA, @OnCarriageATA
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END