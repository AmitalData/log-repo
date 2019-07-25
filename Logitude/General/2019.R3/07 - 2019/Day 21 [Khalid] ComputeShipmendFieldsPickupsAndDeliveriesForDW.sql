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


DECLARE ShipmentsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant
	FROM Shipments	
	OPEN ShipmentsCursor FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN	

set @FirstPickupId = (select top 1 Id from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' order by PickUpDeliveryNumber)
				set @LastDeliveryId = (select top 1 Id from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' order by PickUpDeliveryNumber desc)


if @FirstPickupId is not null
				begin  	
				
				set @FirstpickUpATA = (select top 1 ATA from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' and Id=@FirstPickupId)
set @FirstpickUpATD = (select top 1 ATD from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'PICK' and Id=@FirstPickupId)
					update ShipmentComputedFields set FirstPickupATA=@FirstpickUpATA,FirstPickupATD=@FirstpickUpATD where tenant=@tenant and Id=@ShipmentId
				end


				
if @LastDeliveryId is not null
				begin  		
								set @LastDeliveryATA = (select top 1 ATA from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' and Id=@LastDeliveryId)
								set @LastDeliveryATD = (select top 1 ATD from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' and Id=@LastDeliveryId)
								set @LastDeliveryETA = (select top 1 ETA from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' and Id=@LastDeliveryId)
								set @LastDeliveryETD = (select top 1 ETD from ShipmentPickUpDeliveries where ShipmentId = @ShipmentId and PickUpDeliveryTypeCode = 'DELV' and Id=@LastDeliveryId)

					update ShipmentComputedFields set FinalDeliveryATA=@LastDeliveryATA,FinalDeliveryATD=@LastDeliveryATD,FinalDeliveryETA=@LastDeliveryETA,FinalDeliveryETD=@LastDeliveryETD where tenant=@tenant and Id=@ShipmentId
				end



		FETCH NEXT FROM ShipmentsCursor INTO @ShipmentId, @Tenant
		END
	CLOSE ShipmentsCursor
	DEALLOCATE ShipmentsCursor
	

	
	
