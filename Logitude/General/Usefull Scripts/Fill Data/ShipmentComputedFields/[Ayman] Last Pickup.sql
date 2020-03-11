
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
	Id varchar(15) not null,
	ETA datetime null,
	ETD datetime null,
	ATA datetime null,
	ATD datetime null
)


select
Shipments.Id, ShipmentPickUpDeliveries.ETA, ShipmentPickUpDeliveries.ETD, ShipmentPickUpDeliveries.ATA, ShipmentPickUpDeliveries.ATD
into #temp
from Shipments
left outer join ShipmentPickUpDeliveries
on ShipmentPickUpDeliveries.Id = 
(
	select top 1 Id from ShipmentPickUpDeliveries 
	where ShipmentPickUpDeliveries.ShipmentId = Shipments.Id 
	and PickUpDeliveryTypeCode = 'PICK' 
	order by PickUpDeliveryNumber desc
)
go

	declare @Id as varchar(15)
	declare @ETA as datetime
	declare @ETD as datetime
	declare @ATA as datetime
	declare @ATD as datetime

	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, ETA, ETD, ATA, ATD
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @ETA, @ETD, @ATA, @ATD
	WHILE @@FETCH_STATUS = 0
	BEGIN

		insert into #temp_ShipmentComputedFields(Id, ETA, ETD, ATA, ATD) values(@Id, @ETA, @ETD, @ATA, @ATD)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin		
			update ShipmentComputedFields
			set
			LastPickupETA = #temp_ShipmentComputedFields.ETA,
			LastPickupETD = #temp_ShipmentComputedFields.ETD,
			LastPickupATA = #temp_ShipmentComputedFields.ATA,
			LastPickupATD = #temp_ShipmentComputedFields.ATD
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id

			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end
	FETCH NEXT FROM DataCursor INTO @Id, @ETA, @ETD, @ATA, @ATD
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			LastPickupETA = #temp_ShipmentComputedFields.ETA,
			LastPickupETD = #temp_ShipmentComputedFields.ETD,
			LastPickupATA = #temp_ShipmentComputedFields.ATA,
			LastPickupATD = #temp_ShipmentComputedFields.ATD
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields