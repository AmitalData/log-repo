
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
	[From] nvarchar(40) null,
	[To] nvarchar(40) null
)

select
Shipments.Id,
ShipmentPickUpDeliveries.PickUpDeliveryFromTypeCode as FromTypeCode,
ShipmentPickUpDeliveries.PickUpDeliveryToTypeCode as ToTypeCode,
ShipmentPickUpDeliveries.FromAddressCity as FromCity,
ShipmentPickUpDeliveries.ToAddressCity as ToCity,
FromAddress.City as FromAddressCity,
ToAddress.City as ToAddressCity,
FromPort.EnglishName as FromPortName,
ToPort.EnglishName as ToPortName

into #temp

from Shipments
left outer join ShipmentPickUpDeliveries
on ShipmentPickUpDeliveries.Id = 
(
	select top 1 Id from ShipmentPickUpDeliveries
	where ShipmentPickUpDeliveries.ShipmentId = Shipments.Id 
	and PickUpDeliveryTypeCode = 'PICK' 
	order by PickUpDeliveryNumber
)
left outer join Addresses as FromAddress on FromAddress.Id = ShipmentPickUpDeliveries.FromAddressId
left outer join Addresses as ToAddress on ToAddress.Id = ShipmentPickUpDeliveries.ToAddressId
left outer join Ports as FromPort on FromPort.Id = ShipmentPickUpDeliveries.FromPortId
left outer join Ports as ToPort on ToPort.Id = ShipmentPickUpDeliveries.ToPortId
go

	declare @Id as varchar(15)
	declare @FromTypeCode as varchar(4)
	declare @ToTypeCode as varchar(4)
	declare @FromCity as nvarchar(25)
	declare @ToCity as nvarchar(25)
	declare @FromAddressCity as nvarchar(25)
	declare @ToAddressCity as nvarchar(25)	
	declare @FromPortName as varchar(40)
	declare @ToPortName as varchar(40)
	declare @From as nvarchar(40)
	declare @To as nvarchar(40)

	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, FromTypeCode, ToTypeCode, FromCity, ToCity, FromAddressCity, ToAddressCity, FromPortName, ToPortName
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @FromTypeCode, @ToTypeCode, @FromCity, @ToCity, @FromAddressCity, @ToAddressCity, @FromPortName, @ToPortName
	WHILE @@FETCH_STATUS = 0
	BEGIN

		-- From
		if (@FromTypeCode = 'CASL')
		set @From = @FromCity

		else if (@FromTypeCode = 'PORT')
		set @From = @FromPortName

		else
		set @From = @FromAddressCity

		-- To
		if (@ToTypeCode = 'CASL')
		set @To = @ToCity

		else if (@ToTypeCode = 'PORT')
		set @To = @ToPortName

		else
		set @To = @ToAddressCity

		insert into #temp_ShipmentComputedFields(Id, [From], [To]) values(@Id, @From, @To)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin		
			update ShipmentComputedFields
			set
			PickupFrom = #temp_ShipmentComputedFields.[From],
			PickupTo = #temp_ShipmentComputedFields.[To]
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id

			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end
	FETCH NEXT FROM DataCursor INTO @Id, @FromTypeCode, @ToTypeCode, @FromCity, @ToCity, @FromAddressCity, @ToAddressCity, @FromPortName, @ToPortName
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			PickupFrom = #temp_ShipmentComputedFields.[From],
			PickupTo = #temp_ShipmentComputedFields.[To]
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields