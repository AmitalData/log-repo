begin
declare @Id as varchar(15)
declare @Tenant as int
declare @TotalQuantity as int

	DECLARE WarehouseReleaseCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		From WarehouseReleases
		OPEN WarehouseReleaseCursor FETCH NEXT FROM WarehouseReleaseCursor INTO @Id,  @Tenant
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @TotalQuantity  = (select SUM(Quantity) from WarehouseReleasePackages where WarehouseReleaseId = @Id and Tenant = @Tenant)
		Update  WarehouseReleases set TotalQuantity = @TotalQuantity where Id = @Id and tenant =@Tenant
		
	FETCH NEXT FROM WarehouseReleaseCursor INTO  @Id, @Tenant
	END
	CLOSE WarehouseReleaseCursor
	DEALLOCATE WarehouseReleaseCursor
END

