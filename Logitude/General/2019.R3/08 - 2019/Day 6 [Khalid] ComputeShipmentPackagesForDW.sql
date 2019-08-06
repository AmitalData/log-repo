declare @Tenant as int
declare @ShipmentId as varchar(15)
declare @FirstPickupId varchar(15)
declare @LastDeliveryId varchar(15)
declare @FirstpickUpATA as datetime
declare @FirstpickUpATD as datetime
declare @LastDeliveryATA as datetime
declare @LastDeliveryATD as datetime
declare @LastDeliveryETA as datetime
declare @LastDeliveryETD as datetime
declare @ContainerNumber varchar(4000)
declare @ContainerNumberSum varchar(4000)


DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT shipment.Id, shipment.Tenant,ContainerNumber
	FROM Shipments	shipment inner join ShipmentPackages shipmentpackage  on shipment.Id = shipmentpackage.ShipmentId where shipment.Tenant=1 and shipmentpackage.ContainerNumber is not null order by shipment.Id
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant ,@ContainerNumber
	WHILE @@FETCH_STATUS = 0
		BEGIN	
		if(@ContainerNumber is not null)
		print(@ContainerNumber+' - '+@ShipmentId);
		FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant,@ContainerNumber
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
	

	
	
