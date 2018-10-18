

begin
declare @Id as varchar(15)
declare @PackageTypeId as varchar(15)




	DECLARE WarehouseReleasePackagesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,PackageTypeId
	From WarehouseReleasePackages
	where ((PackageTypeId is not null and Quantity <2) or ContainerNumber is not null) and IsContainer!=1
	OPEN WarehouseReleasePackagesCursor FETCH NEXT FROM WarehouseReleasePackagesCursor INTO @Id,@PackageTypeId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	if(@PackageTypeId is not null)
	BEGIN

	update WarehouseReleasePackages set IsContainer = (select IsContainer from PackageTypes where Id = @PackageTypeId) where Id = @Id
	end
	else 
	BEGIN
	
       update WarehouseReleasePackages set IsContainer =1 where Id = @Id	
	end

	FETCH NEXT FROM WarehouseReleasePackagesCursor INTO @Id,@PackageTypeId
	END
	CLOSE WarehouseReleasePackagesCursor
	DEALLOCATE WarehouseReleasePackagesCursor
END

