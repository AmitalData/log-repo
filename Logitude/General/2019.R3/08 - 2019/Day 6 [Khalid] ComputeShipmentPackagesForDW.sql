declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @ContainerNumberSum varchar(4000)

 
DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT  (STUFF((SELECT CAST(',' + ContainerNumber AS VARCHAR(4000)) 
         FROM ShipmentPackages
         WHERE (Shipments.Id = ShipmentPackages.ShipmentId  and ShipmentPackages.ContainerNumber is not null) 
         FOR XML PATH ('')), 1, 1, '')) AS ContainerNumbers, Id, Tenant
	FROM Shipments	  group by Id,Tenant
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ContainerNumberSum, @ShipmentId, @Tenant 
	WHILE @@FETCH_STATUS = 0
		BEGIN	
		if(@ContainerNumberSum is not null)
		BEGIN
		update ShipmentComputedFields set ContainersNumbers=@ContainerNumberSum where Id=@ShipmentId and Tenant=@Tenant
		END
		FETCH NEXT FROM ShipmentsCursor INTO @ContainerNumberSum,@ShipmentId, @Tenant
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
	
	
	
	
	
	
	
	
	