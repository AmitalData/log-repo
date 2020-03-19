declare @Tenant as int
declare @Id as varchar(15)
declare @WarehouseLegActualEntryDate as datetime
declare @WarehouseLegActualReleaseDate as datetime
declare @WarehouseStorageFreeDays as float
declare @Quantity as float 
declare @GrossWeightPerTon as float

BEGIN 
	DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, GrossWeightPerTon, WarehouseLegActualEntryDate, WarehouseLegActualReleaseDate,WarehouseStorageFreeDays 
	FROM Shipments where GrossWeightPerTon is not null 
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @Id, @Tenant, @GrossWeightPerTon, @WarehouseLegActualEntryDate, @WarehouseLegActualReleaseDate, @WarehouseStorageFreeDays    
	WHILE @@FETCH_STATUS = 0
		BEGIN

		set @Quantity = @GrossWeightPerTon * (SELECT DATEDIFF(day, @WarehouseLegActualEntryDate, @WarehouseLegActualReleaseDate) - @WarehouseStorageFreeDays);

        if (@Quantity < 0)
        begin 
			set @Quantity = 0;
		end 

		update Shipments set GrossWeightPerStorageDays = round(@Quantity, 3) where Id= @Id and Tenant = @Tenant 

        FETCH NEXT FROM ShipmentsCursor INTO  @Id, @Tenant, @GrossWeightPerTon, @WarehouseLegActualEntryDate, @WarehouseLegActualReleaseDate, @WarehouseStorageFreeDays   
        END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
END
