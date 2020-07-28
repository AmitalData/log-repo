
select
SearchFields , EntityId , Tenant
into #DocumentsFilingstemp
from DocumentsFilings where EntityId   in (select id from ShipmentComputedFields where Tenant =  DocumentsFilings.Tenant) and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment') 

If(OBJECT_ID('tempdb..#temp_ShipmentComputedFields') Is Not Null)
Begin
    Drop Table #temp_ShipmentComputedFields
End

CREATE TABLE #temp_ShipmentComputedFields
(
	   Id VARCHAR(15) NULL,
	   DocumentsSearchFields nvarchar(max) NULL,
)



declare  @Tenant int
declare @Count as int
set @Count = 0;

declare  @ShipmentId varchar(15)
	DECLARE ShipmentComputedFieldCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant
	From ShipmentComputedFields
	where DocumentsSearchFields is not null 
	OPEN ShipmentComputedFieldCursor FETCH NEXT FROM ShipmentComputedFieldCursor INTO @ShipmentId , @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN

		begin



declare  @DocumentsSearchFields nvarchar(max) 
set @DocumentsSearchFields = ''

   declare  @SearchFields nvarchar(max) 
	DECLARE DocumentsFilingsCursor CURSOR READ_ONLY
	FOR
	SELECT SearchFields
	From  #DocumentsFilingstemp
	where SearchFields is not null and EntityId = @ShipmentId and Tenant = @Tenant
	OPEN DocumentsFilingsCursor FETCH NEXT FROM DocumentsFilingsCursor INTO @SearchFields
	WHILE @@FETCH_STATUS = 0
	BEGIN

		begin


		if(@DocumentsSearchFields !='') begin set @DocumentsSearchFields = @DocumentsSearchFields + ','  end
		
		set @DocumentsSearchFields = @DocumentsSearchFields + @SearchFields

		end

	FETCH NEXT FROM DocumentsFilingsCursor INTO @SearchFields
	END
	CLOSE DocumentsFilingsCursor
	DEALLOCATE DocumentsFilingsCursor
    
		
		if(@DocumentsSearchFields = '')begin set @DocumentsSearchFields = null end


		insert into #temp_ShipmentComputedFields(Id, [DocumentsSearchFields]) values(@ShipmentId,@DocumentsSearchFields)

	set @Count = @Count + 1;
		if(@Count = 4000)
		begin	

			update ShipmentComputedFields
			set
			DocumentsSearchFields = #temp_ShipmentComputedFields.DocumentsSearchFields
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
			truncate table #temp_ShipmentComputedFields
			set @Count = 0
		end


		end


	FETCH NEXT FROM ShipmentComputedFieldCursor INTO @ShipmentId , @Tenant
	END
	CLOSE ShipmentComputedFieldCursor
	DEALLOCATE ShipmentComputedFieldCursor



		if (@Count > 0)
	begin
			update ShipmentComputedFields
			set
			DocumentsSearchFields = #temp_ShipmentComputedFields.DocumentsSearchFields
			FROM ShipmentComputedFields
			INNER JOIN #temp_ShipmentComputedFields
			on ShipmentComputedFields.Id = #temp_ShipmentComputedFields.Id
	end


drop table #temp_ShipmentComputedFields
	drop table #DocumentsFilingstemp

    

 

	