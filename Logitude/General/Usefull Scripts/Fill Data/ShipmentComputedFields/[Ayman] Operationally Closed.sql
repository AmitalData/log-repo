
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
	UserId varchar(15) null,
	UserName varchar(100) null
)

select
Shipments.Id, Shipments.OperationalClosedByUserId as UserId, Contacts.EnglishName as UserName
into #temp
from Shipments
left outer join Contacts on Shipments.OperationalClosedByUserId = Contacts.Id
go

	declare @Id as varchar(15)
	declare @UserId as varchar(15)
	declare @UserName as varchar(60)

	declare @Count as int
	set @Count = 0;

	DECLARE DataCursor CURSOR READ_ONLY
	FOR
	SELECT Id, UserId, UserName
	FROM #temp	
	OPEN DataCursor FETCH NEXT FROM DataCursor INTO @Id, @UserId, @UserName
	WHILE @@FETCH_STATUS = 0
	BEGIN

		insert into #temp_ShipmentComputedFields(Id, UserId, UserName) values(@Id, @UserId, @UserName)

		set @Count = @Count + 1;
		if(@Count = 1000)
		begin		
			update ShipmentComputedFields
			set
			OperationallyClosedByUserId = #temp_ShipmentComputedFields.UserId,
			OperationallyClosedByUserName = #temp_ShipmentComputedFields.UserName
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id

			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end
	FETCH NEXT FROM DataCursor INTO @Id, @UserId, @UserName
	END
	CLOSE DataCursor
	DEALLOCATE DataCursor


	if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			OperationallyClosedByUserId = #temp_ShipmentComputedFields.UserId,
			OperationallyClosedByUserName = #temp_ShipmentComputedFields.UserName
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end

drop table #temp
drop table #temp_ShipmentComputedFields