
declare @Tenant as int
declare @EntityId as varchar(15)
declare @Main_ETD as datetime
declare @Main_ATD as datetime
declare @Main_ETA as datetime
declare @Main_ATA as datetime
declare @TR1_ETA as datetime
declare @TR1_ATA as datetime
declare @TR2_ETA as datetime
declare @TR2_ATA as datetime
declare @TR3_ETA as datetime
declare @TR3_ATA as datetime
declare @TR1PortId as varchar(15)
declare @TR2PortId as varchar(15)
declare @TR3PortId as varchar(15)
declare @to_ETA as datetime
declare @to_ATA as datetime
declare @DepartureArrivalToDate as datetime
declare @DepartureArrivalFromDate as datetime

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant,
	MainCarriageETD, MainCarriageATD,
	MainCarriageETA, MainCarriageATA,
	Transshipment1ETA, Transshipment1ATA,
	Transshipment2ETA, Transshipment2ATA,
	Transshipment3ETA, Transshipment3ATA,
	Transshipment1ToPortId, Transshipment2ToPortId, Transshipment3ToPortId
	FROM ShipmentMasterDatas
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Main_ETD, @Main_ATD, @Main_ETA, @Main_ATA, @TR1_ETA, @TR1_ATA, @TR2_ETA, @TR2_ATA, @TR3_ETA, @TR3_ATA, @TR1PortId, @TR2PortId, @TR3PortId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @DepartureArrivalFromDate = @Main_ETD
		if (@Main_ATD is not null) set @DepartureArrivalFromDate = @Main_ATD
		
		set @to_ETA = @Main_ETA
		set @to_ATA = @Main_ATA
		
		if (@TR1PortId is not null)
		begin
			if (@TR1_ETA is not null) set @to_ETA = @TR1_ETA
			if (@TR1_ATA is not null) set @to_ATA = @TR1_ATA
		end

		if (@TR2PortId is not null)
		begin
			if (@TR2_ETA is not null) set @to_ETA = @TR2_ETA
			if (@TR2_ATA is not null) set @to_ATA = @TR2_ATA
			
		end

		if (@TR3PortId is not null)
		begin
			if (@TR3_ETA is not null) set @to_ETA = @TR3_ETA
			if (@TR3_ATA is not null) set @to_ATA = @TR3_ATA
		end

		set @DepartureArrivalToDate = @to_ETA
		if (@to_ATA is not null)
		begin				
			if (@DepartureArrivalToDate is null)
			begin
				set @DepartureArrivalToDate = @to_ATA				
			end

			else if (@to_ATA > @DepartureArrivalToDate)
			begin			
				set @DepartureArrivalToDate = @to_ATA
			end
		end

		update ShipmentMasterDatas
		set
		DepartureArrivalFromDate = @DepartureArrivalFromDate,
		DepartureArrivalToDate = @DepartureArrivalToDate,
		MainCarriageFinalDestinationETA = @to_ETA,
		MainCarriageFinalDestinationATA = @to_ATA
		where Id = @EntityId AND Tenant = @Tenant

		--print 'From:' + isnull(convert(varchar, @DepartureArrivalFromDate),'null')
		--print 'To_ETA:' + isnull(convert(varchar, @to_ETA),'null')
		--print 'To_ATA:' + isnull(convert(varchar, @to_ATA),'null')
		--print 'To_Final:' + isnull(convert(varchar, @DepartureArrivalToDate),'null')

	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Main_ETD, @Main_ATD, @Main_ETA, @Main_ATA, @TR1_ETA, @TR1_ATA, @TR2_ETA, @TR2_ATA, @TR3_ETA, @TR3_ATA, @TR1PortId, @TR2PortId, @TR3PortId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
