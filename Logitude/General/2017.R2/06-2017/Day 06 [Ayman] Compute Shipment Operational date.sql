
declare @Tenant as int
declare @Id as varchar(15)
declare @DirectionId as varchar(1)
declare @CreateDateTime as datetime
declare @MainCarriageETA as datetime
declare @Transshipment1ETA as datetime
declare @Transshipment2ETA as datetime
declare @Transshipment3ETA as datetime
declare @MainCarriageATA as datetime
declare @Transshipment1ATA as datetime
declare @Transshipment2ATA as datetime
declare @Transshipment3ATA as datetime
declare @MainCarriageETD as datetime
declare @Transshipment1ETD as datetime
declare @Transshipment2ETD as datetime
declare @Transshipment3ETD as datetime
declare @MainCarriageATD as datetime
declare @Transshipment1ATD as datetime
declare @Transshipment2ATD as datetime
declare @Transshipment3ATD as datetime
declare @OperationalDate as datetime

BEGIN
	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, DirectionId, CreateDateTime
	FROM Shipments
	where OperationalDate is null
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @DirectionId, @CreateDateTime
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @OperationalDate = null

		if(@DirectionId = 'I')
		begin
			SELECT 
			@MainCarriageETA = MainCarriageETA,
			@Transshipment1ETA = Transshipment1ETA,
			@Transshipment2ETA = Transshipment2ETA,
			@Transshipment3ETA = Transshipment3ETA,
			@MainCarriageATA = MainCarriageATA,
			@Transshipment1ATA = Transshipment1ATA,
			@Transshipment2ATA = Transshipment2ATA,
			@Transshipment3ATA = Transshipment3ATA
			from ShipmentMasterDatas where Id = @Id AND Tenant = @Tenant

			-- Actual Arrival
			if (@Transshipment3ATA is not null)
            set @OperationalDate = @Transshipment3ATA;
                
            else if (@Transshipment2ATA is not null)
            set @OperationalDate = @Transshipment2ATA;
                
            else if (@Transshipment1ATA is not null)
            set @OperationalDate = @Transshipment1ATA;

            else if (@MainCarriageATA is not null)
            set @OperationalDate = @MainCarriageATA;

			-- Expected Arrival
			else if (@Transshipment3ETA is not null)
            set @OperationalDate = @Transshipment3ETA;

			else if (@Transshipment2ETA is not null)
            set @OperationalDate = @Transshipment2ETA;

			else if (@Transshipment1ETA is not null)
            set @OperationalDate = @Transshipment1ETA;

            else if (@MainCarriageETA is not null)
            set @OperationalDate = @MainCarriageETA;

			else
            set @OperationalDate = @CreateDateTime;
		end

		else
		begin			
			SELECT 
			@MainCarriageETD = MainCarriageETD,
			@Transshipment1ETD = Transshipment1ETD,
			@Transshipment2ETD = Transshipment2ETD,
			@Transshipment3ETD = Transshipment3ETD,
			@MainCarriageATD = MainCarriageATD,
			@Transshipment1ATD = Transshipment1ATD,
			@Transshipment2ATD = Transshipment2ATD,
			@Transshipment3ATD = Transshipment3ATD
			from ShipmentMasterDatas where Id = @Id AND Tenant = @Tenant

			-- Actual Departure
			if (@MainCarriageATD is not null)
			set @OperationalDate = @MainCarriageATD;

			else if (@Transshipment1ATD is not null)
			set @OperationalDate = @Transshipment1ATD;

			else if (@Transshipment2ATD is not null)
			set @OperationalDate = @Transshipment2ATD;

			else if (@Transshipment3ATD is not null)
			set @OperationalDate = @Transshipment3ATD;

			-- Expected Departure
			else if (@MainCarriageETD is not null)
			set @OperationalDate = @MainCarriageETD;

			else if (@Transshipment1ETD is not null)
			set @OperationalDate = @Transshipment1ETD;

			else if (@Transshipment2ETD is not null)
			set @OperationalDate = @Transshipment2ETD;

			else if (@Transshipment3ETD is not null)
			set @OperationalDate = @Transshipment3ETD;

			else
            set @OperationalDate = @CreateDateTime;

		end

		update Shipments set OperationalDate = @OperationalDate where Id = @Id AND Tenant = @Tenant

	FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @DirectionId, @CreateDateTime
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor
END