
If(OBJECT_ID('tempdb..#temp') Is Not Null)
Begin
    Drop Table #temp
End

If(OBJECT_ID('tempdb..#temp_ShipmentComputedFields') Is Not Null)
Begin
    Drop Table #temp_ShipmentComputedFields
End

CREATE TABLE #temp_ShipmentComputedFields
(
	   Id VARCHAR(15) NULL,
	   PickupTruckerId VARCHAR(15) NULL,
	   PickupTruckerNumber VARCHAR(15) NULL,
	   PickupDriver VARCHAR(40) NULL,
	   PickupTrailerNumber VARCHAR(15) NULL,
	   PickupNotes NVARCHAR(2000) NULL,
	   DeliveryTruckerId VARCHAR(15) NULL,
	   DeliveryTruckerNumber VARCHAR(15) NULL,
	   DeliveryDriver VARCHAR(40) NULL,
	   DeliveryTrailerNumber VARCHAR(15) NULL,
	   DeliveryNotes NVARCHAR(2000) NULL,
)

select
Shipments.Id,
ShipmentPickUps.CarrierId as PickupTruckerId,
ShipmentPickUps.CarrierNumber as PickupTruckerNumber,
ShipmentPickUps.Driver as PickupDriver,
ShipmentPickUps.TrailerNumber as PickupTrailerNumber,
ShipmentPickUps.Notes as PickupNotes,
ShipmentDeliveries.CarrierId as DeliveryTruckerId,
ShipmentDeliveries.CarrierNumber as DeliveryTruckerNumber ,
ShipmentDeliveries.Driver as DeliveryDriver,
ShipmentDeliveries.TrailerNumber as DeliveryTrailerNumber,
ShipmentDeliveries.Notes as DeliveryNotes
into #temp

from Shipments
left outer join ShipmentPickUpDeliveries ShipmentPickUps
on ShipmentPickUps.Id = 
(
	select top 1 Id from ShipmentPickUpDeliveries
	where ShipmentPickUpDeliveries.ShipmentId = Shipments.Id 
	and PickUpDeliveryTypeCode = 'PICK' 
	order by PickUpDeliveryNumber
)
left outer join ShipmentPickUpDeliveries ShipmentDeliveries
on ShipmentDeliveries.Id = 
(
	select top 1 Id from ShipmentPickUpDeliveries
	where ShipmentPickUpDeliveries.ShipmentId = Shipments.Id 
	and PickUpDeliveryTypeCode = 'DELV' 
	order by PickUpDeliveryNumber desc
)

go

	declare @Id as varchar(15)


       declare @PickupTruckerId as VARCHAR(15) 
	   declare @PickupTruckerNumber as VARCHAR(15) 
	   declare @PickupDriver as  VARCHAR(40) 
	   declare @PickupTrailerNumber as VARCHAR(15) 
	   declare @PickupNotes as NVARCHAR(2000) 
	   declare @DeliveryTruckerId as VARCHAR(15) 
	   declare @DeliveryTruckerNumber as  VARCHAR(15) 
	   declare @DeliveryDriver  as VARCHAR(40) 
	   declare @DeliveryTrailerNumber  as VARCHAR(15) 
	   declare @DeliveryNotes as NVARCHAR(2000) 






	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, PickupTruckerId, PickupTruckerNumber, PickupDriver, PickupTrailerNumber, PickupNotes,  DeliveryTruckerId , DeliveryTruckerNumber, DeliveryDriver, DeliveryTrailerNumber, DeliveryNotes
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id,@PickupTruckerId, @PickupTruckerNumber, @PickupDriver, @PickupTrailerNumber, @PickupNotes,@DeliveryTruckerId, @DeliveryTruckerNumber, @DeliveryDriver, @DeliveryTrailerNumber, @DeliveryNotes
	WHILE @@FETCH_STATUS = 0
	BEGIN

		

		insert into #temp_ShipmentComputedFields(Id, [PickupTruckerId] , [PickupTruckerNumber], [PickupDriver],[PickupTrailerNumber] , [PickupNotes], [DeliveryTruckerId] , [DeliveryTruckerNumber], [DeliveryDriver],[DeliveryTrailerNumber] , [DeliveryNotes]) values(@Id, @PickupTruckerId, @PickupTruckerNumber, @PickupDriver, @PickupTrailerNumber, @PickupNotes,@DeliveryTruckerId, @DeliveryTruckerNumber, @DeliveryDriver, @DeliveryTrailerNumber, @DeliveryNotes)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin		
			update ShipmentComputedFields
			set
			PickupTruckerId = #temp_ShipmentComputedFields.[PickupTruckerId],
			PickupTruckerNumber = #temp_ShipmentComputedFields.PickupTruckerNumber,
			PickupDriver = #temp_ShipmentComputedFields.PickupDriver,
			PickupTrailerNumber = #temp_ShipmentComputedFields.PickupTrailerNumber,
			PickupNotes = #temp_ShipmentComputedFields.PickupNotes,
			DeliveryTruckerId = #temp_ShipmentComputedFields.DeliveryTruckerId,
			DeliveryTruckerNumber = #temp_ShipmentComputedFields.DeliveryTruckerNumber,
			DeliveryDriver = #temp_ShipmentComputedFields.DeliveryDriver,
			DeliveryTrailerNumber = #temp_ShipmentComputedFields.DeliveryTrailerNumber,
			DeliveryNotes = #temp_ShipmentComputedFields.DeliveryNotes

			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id

			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end
	FETCH NEXT FROM DataCursor  INTO @Id,@PickupTruckerId, @PickupTruckerNumber, @PickupDriver, @PickupTrailerNumber, @PickupNotes,@DeliveryTruckerId, @DeliveryTruckerNumber, @DeliveryDriver, @DeliveryTrailerNumber, @DeliveryNotes
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			PickupTruckerId = #temp_ShipmentComputedFields.[PickupTruckerId],
			PickupTruckerNumber = #temp_ShipmentComputedFields.PickupTruckerNumber,
			PickupDriver = #temp_ShipmentComputedFields.PickupDriver,
			PickupTrailerNumber = #temp_ShipmentComputedFields.PickupTrailerNumber,
			PickupNotes = #temp_ShipmentComputedFields.PickupNotes,
			DeliveryTruckerId = #temp_ShipmentComputedFields.DeliveryTruckerId,
			DeliveryTruckerNumber = #temp_ShipmentComputedFields.DeliveryTruckerNumber,
			DeliveryDriver = #temp_ShipmentComputedFields.DeliveryDriver,
			DeliveryTrailerNumber = #temp_ShipmentComputedFields.DeliveryTrailerNumber,
			DeliveryNotes = #temp_ShipmentComputedFields.DeliveryNotes

			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields