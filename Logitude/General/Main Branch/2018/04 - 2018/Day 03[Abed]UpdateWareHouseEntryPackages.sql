


begin
declare @Id as varchar(15)
declare @PackageTypeId as varchar(15)




	DECLARE WarehouseEntryPackagesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,PackageTypeId
	From WarehouseEntryPackages
	where ((PackageTypeId is not null and Quantity <2) or ContainerNumber is not null) and IsContainer!=1
	OPEN WarehouseEntryPackagesCursor FETCH NEXT FROM WarehouseEntryPackagesCursor INTO @Id,@PackageTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	if(@PackageTypeId is not null)
	BEGIN
	update WarehouseEntryPackages set IsContainer = (select IsContainer from PackageTypes where Id = @PackageTypeId) where Id = @Id
	end
	else 
	BEGIN

       update WarehouseEntryPackages set IsContainer =1 where Id = @Id	
	end

	FETCH NEXT FROM WarehouseEntryPackagesCursor INTO @Id,@PackageTypeId
	END
	CLOSE WarehouseEntryPackagesCursor
	DEALLOCATE WarehouseEntryPackagesCursor
END

