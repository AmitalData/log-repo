
IF OBJECT_ID('[dbo].[usp_UpdateShipmentFinalArrivalDate]', 'P') IS NOT NULL
drop PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
GO

Create PROCEDURE [dbo].[usp_UpdateShipmentFinalArrivalDate]
(
	@ShipmentId varchar(15)
)
AS


declare @Tenant as int
declare @ShipmentLevelCode as varchar(1)
declare @MasterShipmentDataId as varchar(15)
declare @DirectionId as varchar(3)
declare @TransportModeId as varchar(3)

--Delivery
declare @ETA as datetime
declare @ATA as datetime
declare @DeliveryDate as datetime

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
declare @DeliveriesDate as datetime
declare @OnCarriageDate as datetime
declare @Transshipment1Date as datetime
declare @Transshipment2Date as datetime
declare @Transshipment3Date as datetime
declare @MainCarriageDate as datetime
declare @FinalArrivalDate as datetime
declare @EstimatedFinalArrivalDate as datetime
declare @ActualFinalArrivalDate as datetime

BEGIN

	SELECT
	@Tenant = Tenant,
	@ShipmentLevelCode = ShipmentLevelCode,
	@DirectionId = DirectionId,
	@TransportModeId = TransportModeId,
	@MasterShipmentDataId = MasterShipmentDataId,
	@OnCarriageFromPortId = OnCarriageFromPortId,
	@OnCarriageToPortId = OnCarriageToPortId,
	@OnCarriageETA = OnCarriageETA,
	@OnCarriageATA = OnCarriageATA
	from Shipments where Id = @ShipmentId

	set @EstimatedDeliveriesDate = null
	set @ActualDeliveriesDate = null
	set @DeliveriesDate = null
	set @OnCarriageDate = null
	set @Transshipment1Date = null
	set @Transshipment2Date = null
	set @Transshipment3Date = null
	set @MainCarriageDate = null
	set @FinalArrivalDate = null
	set @EstimatedFinalArrivalDate = null
	set @ActualFinalArrivalDate = null

	-- @DeliveriesDate
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

			set @DeliveryDate = null
			
			if (@ATA is not null)
			set @DeliveryDate = @ATA

			else if (@ETA is not null)
			set @DeliveryDate = @ETA

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
	END

	-- @OnCarriageDate
	if (@OnCarriageFromPortId is not null and @OnCarriageToPortId is not null)
	BEGIN
		if (@OnCarriageATA is not null)
		set @OnCarriageDate = @OnCarriageATA

		else if (@OnCarriageETA is not null)
		set @OnCarriageDate = @OnCarriageETA	
	END

	--Estimated
	if (@EstimatedDeliveriesDate is not null)
	set @EstimatedFinalArrivalDate = @EstimatedDeliveriesDate

	else if (@OnCarriageETA is not null)
	set @EstimatedFinalArrivalDate = @OnCarriageETA
	
	--Actual	
	if (@ActualDeliveriesDate is not null)
	set @ActualFinalArrivalDate = @ActualDeliveriesDate
	
	else if (@OnCarriageATA is not null)
	set @ActualFinalArrivalDate = @OnCarriageATA

	--Estimated Or ACtual
	if (@DeliveriesDate is not null)
	set @FinalArrivalDate = @DeliveriesDate

	else if (@OnCarriageDate is not null)
	set @FinalArrivalDate = @OnCarriageDate

	if (@ShipmentLevelCode = 'H')
	BEGIN
		-- if not connected: then fiels is computed above
		-- if connected but the master field is null while the house field is not (above computed field)
		-- if connected but the master got no deliveries / on carriage while the house has (above computed field)
		-- for now just take the field from the master
		if(@MasterShipmentDataId is not null)
		begin		
			set @FinalArrivalDate = (select FinalArrivalDate from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant)		
			set @EstimatedFinalArrivalDate = (select EstimatedFinalArrivalDate from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant)
			set @ActualFinalArrivalDate = (select ActualFinalArrivalDate from Shipments where Id = @MasterShipmentDataId and Tenant = @Tenant)
		end
	END

	else if (@FinalArrivalDate is null)
	BEGIN
		-- only if the above computed field is null then compute from legs
		print 'ddd'
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
			set @Transshipment3Date = @Transshipment3ATA

			else if (@Transshipment3ETA is not null)
			set @Transshipment3Date = @Transshipment3ETA	
		END

		-- @Transshipment2
		if (@Transshipment2FromPortId is not null and @Transshipment2ToPortId is not null)
		BEGIN
		
			if (@Transshipment2ATA is not null)
			set @Transshipment2Date = @Transshipment2ATA

			else if (@Transshipment2ETA is not null)
			set @Transshipment2Date = @Transshipment2ETA	
		END

		-- @Transshipment1
		if (@Transshipment1FromPortId is not null and @Transshipment1ToPortId is not null)
		BEGIN
		
			if (@Transshipment1ATA is not null)
			set @Transshipment1Date = @Transshipment1ATA

			else if (@Transshipment1ETA is not null)
			set @Transshipment1Date = @Transshipment1ETA	
		END

		-- @MainCarriage
		else
		BEGIN
		
			if (@MainCarriageATA is not null)
			set @MainCarriageDate = @MainCarriageATA

			else if (@MainCarriageETA is not null)
			set @MainCarriageDate = @MainCarriageETA	
		END

		--Estimated
		if (@Transshipment3ETA is not null)
		set @EstimatedFinalArrivalDate = @Transshipment3ETA

		else if (@Transshipment2ETA is not null)
		set @EstimatedFinalArrivalDate = @Transshipment2ETA

		else if (@Transshipment1ETA is not null)
		set @EstimatedFinalArrivalDate = @Transshipment1ETA

		else
		set @EstimatedFinalArrivalDate = @MainCarriageETA

		--Actual
		if (@Transshipment3ATA is not null)
		set @ActualFinalArrivalDate = @Transshipment3ATA

		else if (@Transshipment2ATA is not null)
		set @ActualFinalArrivalDate = @Transshipment2ATA

		else if (@Transshipment1ATA is not null)
		set @ActualFinalArrivalDate = @Transshipment1ATA

		else
		set @ActualFinalArrivalDate = @MainCarriageATA

		--Estimated or Actual
		if (@Transshipment3Date is not null)
		set @FinalArrivalDate = @Transshipment3Date

		else if (@Transshipment2Date is not null)
		set @FinalArrivalDate = @Transshipment2Date

		else if (@Transshipment1Date is not null)
		set @FinalArrivalDate = @Transshipment1Date

		else
		set @FinalArrivalDate = @MainCarriageDate
	END

	print @EstimatedFinalArrivalDate
	update Shipments set FinalArrivalDate = @FinalArrivalDate, EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate, ActualFinalArrivalDate = @ActualFinalArrivalDate 
	where Id = @ShipmentId and Tenant = @Tenant

	if (@ShipmentLevelCode = 'C')
	begin
	-- on connect / diconnect house we should compute
	-- loop houses if maser daet bigger than house date

                                --if (entityPoco.FinalArrivalDate != null && entityPoco.FinalArrivalDate > item.FinalArrivalDate)
                                --{
                                    --item.FinalArrivalDate = entityPoco.FinalArrivalDate;
                                --}

		update Shipments set FinalArrivalDate = @FinalArrivalDate , EstimatedFinalArrivalDate = @EstimatedFinalArrivalDate, ActualFinalArrivalDate = @ActualFinalArrivalDate
	    where MasterShipmentDataId = @ShipmentId and Tenant = @Tenant and ShipmentLevelCode = 'H'
	end
END

