
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
	NumberOfDeliveries int null,
)

select
Shipments.Id, SUM(CASE WHEN Deliveries.ShipmentId IS NULL THEN 0 ELSE 1 END) as NumberOfDeliveries
into #temp
from Shipments
left outer join (select ShipmentId from ShipmentPickUpDeliveries where PickUpDeliveryTypeCode = 'DELV') as Deliveries
on Shipments.Id = Deliveries.ShipmentId
group by Shipments.Id
go

	declare @Id as varchar(15)
	declare @NumberOfDeliveries as int

	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, NumberOfDeliveries
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @NumberOfDeliveries
	WHILE @@FETCH_STATUS = 0
	BEGIN

		if(@NumberOfDeliveries = 0)
		set @NumberOfDeliveries = null

		insert into #temp_ShipmentComputedFields(Id, NumberOfDeliveries) values(@Id, @NumberOfDeliveries)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin		
			update ShipmentComputedFields
			set
			NumberOfDeliveries = #temp_ShipmentComputedFields.NumberOfDeliveries
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id

			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end
	FETCH NEXT FROM DataCursor INTO @Id, @NumberOfDeliveries
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			NumberOfDeliveries = #temp_ShipmentComputedFields.NumberOfDeliveries
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields