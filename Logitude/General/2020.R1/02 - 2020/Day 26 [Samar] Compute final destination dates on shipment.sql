
declare @Tenant as int
declare @EntityId as varchar(15)
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

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant,	
	MainCarriageETA, MainCarriageATA,
	Transshipment1ETA, Transshipment1ATA,
	Transshipment2ETA, Transshipment2ATA,
	Transshipment3ETA, Transshipment3ATA,
	Transshipment1ToPortId, Transshipment2ToPortId, Transshipment3ToPortId
	FROM ShipmentMasterDatas
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Main_ETA, @Main_ATA, @TR1_ETA, @TR1_ATA, @TR2_ETA, @TR2_ATA, @TR3_ETA, @TR3_ATA, @TR1PortId, @TR2PortId, @TR3PortId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @to_ETA = NULL
		set @to_ATA = NULL

		if (@TR3PortId is not null)
		begin
			set @to_ETA = @TR3_ETA
			set @to_ATA = @TR3_ATA
		end			

		else if (@TR2PortId is not null)
		begin
			set @to_ETA = @TR2_ETA
			set @to_ATA = @TR2_ATA			
		end

		else if (@TR1PortId is not null)
		begin
			set @to_ETA = @TR1_ETA
			set @to_ATA = @TR1_ATA
		end

		else
		begin
			set @to_ETA = @Main_ETA
			set @to_ATA = @Main_ATA
		end
		
		update ShipmentMasterDatas
		set
		MainCarriageFinalDestinationETA = @to_ETA,
		MainCarriageFinalDestinationATA = @to_ATA
		where Id = @EntityId AND Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Main_ETA, @Main_ATA, @TR1_ETA, @TR1_ATA, @TR2_ETA, @TR2_ATA, @TR3_ETA, @TR3_ATA, @TR1PortId, @TR2PortId, @TR3PortId
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END
