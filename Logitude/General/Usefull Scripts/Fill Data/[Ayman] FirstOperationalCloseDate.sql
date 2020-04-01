
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
	Tenant int not null,
	FirstOperationalCloseDate datetime
)

select
Id, Tenant, ShipmentLevelCode, OperationalCloseDate
into #temp
from Shipments
where IsOperationalClosed = 1
and FirstOperationalCloseDate is null
and ShipmentLevelCode <> 'H'
go

	declare @EventTypeId as varchar(15)
	declare @ObjectTableId1 as varchar(15)
	declare @ObjectTableId2 as varchar(15)
	set @ObjectTableId1 = (select Id from ObjectTables where Name = 'Master')
	set @ObjectTableId2 = (select Id from ObjectTables where Name = 'Shipment')

	declare @Id as varchar(15)
	declare @Tenant as int
	declare @ShipmentLevelCode as varchar(1)
	declare @OperationalCloseDate as datetime	
	declare @FirstOperationalCloseDate as datetime

	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, ShipmentLevelCode, OperationalCloseDate
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @ShipmentLevelCode, @OperationalCloseDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @EventTypeId = (select Id from EventTypes where Code = 'OPCL' AND Tenant = @Tenant AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2))

		set @FirstOperationalCloseDate =
		(select top 1 LogDateTime from TraceEvents 
		where
		EntityId = @Id
		AND Tenant = @Tenant
		AND EventTypeId = @EventTypeId
		AND ObjectTableId in (@ObjectTableId1, @ObjectTableId2)
		order by LogDateTime)
		
		if (@FirstOperationalCloseDate is null)
		set @FirstOperationalCloseDate = @OperationalCloseDate

		if (@FirstOperationalCloseDate is not null)
		begin
		--print @Id

			if (@ShipmentLevelCode = 'C')
			update Shipments set FirstOperationalCloseDate = @FirstOperationalCloseDate where Tenant = @Tenant AND ShipmentLevelCode = 'H' AND MasterShipmentDataId = @Id

			insert into #temp_ShipmentComputedFields(Id, Tenant, FirstOperationalCloseDate) values(@Id, @Tenant, @FirstOperationalCloseDate)

			set @Count = @Count + 1;
			if(@Count = 1000)
			begin		
				update Shipments
				set
				FirstOperationalCloseDate = #temp_ShipmentComputedFields.FirstOperationalCloseDate
				FROM Shipments
				INNER JOIN #temp_ShipmentComputedFields
				on Shipments.Id = #temp_ShipmentComputedFields.Id

				truncate table #temp_ShipmentComputedFields
				set @Count = 0
			end
		end
	FETCH NEXT FROM DataCursor INTO @Id, @Tenant, @ShipmentLevelCode, @OperationalCloseDate
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor

	if (@Count > 0)
	begin
				update Shipments
				set
				FirstOperationalCloseDate = #temp_ShipmentComputedFields.FirstOperationalCloseDate
				FROM Shipments
				INNER JOIN #temp_ShipmentComputedFields
				on Shipments.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields