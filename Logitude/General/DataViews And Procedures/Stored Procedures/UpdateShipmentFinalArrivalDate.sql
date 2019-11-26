
IF OBJECT_ID('[dbo].[usp_UpdateShipmentFinalArrivalDate]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
GO

Create PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
(
	@ShipmentId varchar(15)
)
AS

if (@ShipmentId is not null)
BEGIN

	-- Shipment Fields
	declare @Tenant as int
	declare @DirectionId as varchar(3)
	declare @TransportModeId as varchar(3)
	declare @ShipmentLevelCode as varchar(1)
	declare @MasterShipmentDataId as varchar(15)
	declare @OnCarriageFromPortId as varchar(15)
	declare @OnCarriageToPortId as varchar(15)
	declare @OnCarriageETA as datetime
	declare @OnCarriageATA as datetime
	
	SELECT
	@Tenant = Tenant,
	@DirectionId = DirectionId,
	@TransportModeId = TransportModeId,
	@ShipmentLevelCode = ShipmentLevelCode,	
	@MasterShipmentDataId = MasterShipmentDataId,
	@OnCarriageFromPortId = OnCarriageFromPortId,
	@OnCarriageToPortId = OnCarriageToPortId,
	@OnCarriageETA = OnCarriageETA,
	@OnCarriageATA = OnCarriageATA
	from Shipments where Id = @ShipmentId

	declare @FinalArrivalDate as datetime	
	declare @ActualFinalArrivalDate as datetime
	declare @EstimatedFinalArrivalDate as datetime

	declare @HasDelivery as bit
	declare @HasOnCarriage as bit

	set @HasDelivery = 0
	set @HasOnCarriage= 0

	-- @DeliveriesDate
	if exists (select * from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and Tenant = @Tenant and PickUpDeliveryTypeCode = 'DELV')
	BEGIN

		set @HasDelivery = 1

		declare @ETA as datetime
		declare @ATA as datetime
		declare @DeliveryDate as datetime
		declare @DeliveriesDate as datetime	
		declare @ActualDeliveriesDate as datetime
		declare @EstimatedDeliveriesDate as datetime

		DECLARE DeliveriesCursor CURSOR READ_ONLY
		FOR
		SELECT ETA, ATA
		FROM ShipmentPickUpDeliveries
		WHERE ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV'
		OPEN DeliveriesCursor FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @DeliveryDate = null
			
			if (@ATA is not null)
			set @DeliveryDate = @ATA

			else if (@ETA is not null)
			set @DeliveryDate = @ETA

			if (@ATA is not null)
			begin
				if (@ActualDeliveriesDate is null)
				set @ActualDeliveriesDate = @ATA

				else if (@ATA > @ActualDeliveriesDate)
				set @ActualDeliveriesDate = @ATA 
			end

			if (@ETA is not null)
			begin
				if (@EstimatedDeliveriesDate is null)
				set @EstimatedDeliveriesDate = @ETA

				else if (@ETA > @EstimatedDeliveriesDate)
				set @EstimatedDeliveriesDate = @ETA 
			end

			if (@DeliveryDate is not null)
			begin
				if (@DeliveriesDate is null)
				set @DeliveriesDate = @DeliveryDate

				else if (@DeliveryDate > @DeliveriesDate)
				set @DeliveriesDate = @DeliveryDate 
			end
		FETCH NEXT FROM DeliveriesCursor INTO @ETA, @ATA
		END
		CLOSE DeliveriesCursor
		DEALLOCATE DeliveriesCursor

		set @FinalArrivalDate = @DeliveriesDate
		set @ActualFinalArrivalDate = @ActualDeliveriesDate
		set @EstimatedFinalArrivalDate = @EstimatedDeliveriesDate
	END

	-- @OnCarriageDate	
	else if (@OnCarriageFromPortId is not null and @OnCarriageToPortId is not null)
	BEGIN
		set @HasOnCarriage = 1

		if (@OnCarriageATA is not null)
		begin
			set @ActualFinalArrivalDate = @OnCarriageATA
			set @FinalArrivalDate = @OnCarriageATA
		end

		if (@OnCarriageETA is not null)
		begin
			set @EstimatedFinalArrivalDate = @OnCarriageETA

			if (@FinalArrivalDate is null)
				set @FinalArrivalDate = @OnCarriageETA
		end
	END

	-- House
		if (@ShipmentLevelCode = 'H' AND @MasterShipmentDataId is not null and @HasDelivery = 0)
		BEGIN	
			declare @IsTakingMasterDates as bit
			set @IsTakingMasterDates = 0;

			if(@HasOnCarriage = 0)
			begin
				set @IsTakingMasterDates = 1
			end

			else
			begin
				if exists (select * from ShipmentPickUpDeliveries where ShipmentId = @MasterShipmentDataId and Tenant = @Tenant and PickUpDeliveryTypeCode = 'DELV')
					set @IsTakingMasterDates = 1
			end

			if (@IsTakingMasterDates = 1)
			begin
				SELECT
				@FinalArrivalDate = FinalArrivalDate,
				@EstimatedFinalArrivalDate = EstimatedFinalArrivalDate,
				@ActualFinalArrivalDate = ActualFinalArrivalDate
				from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant
			end			
		END

		else if (@HasDelivery = 0 AND @HasOnCarriage = 0)
		BEGIN

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

		select		
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
				begin
					set @ActualFinalArrivalDate = @Transshipment3ATA
					set @FinalArrivalDate = @Transshipment3ATA
				end

				if (@Transshipment3ETA is not null)
				begin
					set @EstimatedFinalArrivalDate = @Transshipment3ETA

					if (@FinalArrivalDate is null)
						set @FinalArrivalDate = @Transshipment3ETA
				end
			END

			-- @Transshipment2	
			else if (@Transshipment2FromPortId is not null and @Transshipment2ToPortId is not null)
			BEGIN
				if (@Transshipment2ATA is not null)
				begin
					set @ActualFinalArrivalDate = @Transshipment2ATA
					set @FinalArrivalDate = @Transshipment2ATA
				end

				if (@Transshipment2ETA is not null)
				begin
					set @EstimatedFinalArrivalDate = @Transshipment2ETA

					if (@FinalArrivalDate is null)
						set @FinalArrivalDate = @Transshipment2ETA
				end
			END

			-- @Transshipment1	
			else if (@Transshipment1FromPortId is not null and @Transshipment1ToPortId is not null)
			BEGIN
				if (@Transshipment1ATA is not null)
				begin
					set @ActualFinalArrivalDate = @Transshipment1ATA
					set @FinalArrivalDate = @Transshipment1ATA
				end

				if (@Transshipment1ETA is not null)
				begin
					set @EstimatedFinalArrivalDate = @Transshipment1ETA

					if (@FinalArrivalDate is null)
						set @FinalArrivalDate = @Transshipment1ETA
				end
			END

			-- @MainCarriage	
			else if (@MainCarriageFromPortId is not null and @MainCarriageToPortId is not null)
			BEGIN
				if (@MainCarriageATA is not null)
				begin
					set @ActualFinalArrivalDate = @MainCarriageATA
					set @FinalArrivalDate = @MainCarriageATA
				end

				if (@MainCarriageETA is not null)
				begin
					set @EstimatedFinalArrivalDate = @MainCarriageETA

					if (@FinalArrivalDate is null)
						set @FinalArrivalDate = @MainCarriageETA
				end
			END

		END	

	update Shipments
	set
	FinalArrivalDate = @FinalArrivalDate,
	EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate,
	ActualFinalArrivalDate = @ActualFinalArrivalDate 
	where Id = @ShipmentId and Tenant = @Tenant

	if (@ShipmentLevelCode = 'C')
	BEGIN
		-- Loop Houses
		declare @HouseId as varchar(15)
		DECLARE HousesCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Shipments
		WHERE ShipmentLevelCode = 'H' AND MasterShipmentDataId = @ShipmentId
		OPEN HousesCursor FETCH NEXT FROM HousesCursor INTO @HouseId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			EXECUTE usp_UpdateShipmentFinalArrivalDate @HouseId

		FETCH NEXT FROM HousesCursor INTO @HouseId
		END
		CLOSE HousesCursor
		DEALLOCATE HousesCursor
	END
END