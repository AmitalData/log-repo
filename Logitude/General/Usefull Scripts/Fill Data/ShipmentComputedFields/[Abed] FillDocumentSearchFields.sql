
select
SearchFields , EntityId , Tenant
into #DocumentsFilingstemp
from DocumentsFilings where EntityId   in (select id from ShipmentComputedFields where Tenant =  DocumentsFilings.Tenant) and ObjectTableId = (select Id from ObjectTables where Name = 'Shipment') 

declare  @Tenant int

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

declare  @DocumentsSearchFields varchar(8000) 
set @DocumentsSearchFields = ''

   declare  @SearchFields varchar(8000)
	DECLARE DocumentsFilingsCursor CURSOR READ_ONLY
	FOR
	SELECT SearchFields
	From  #DocumentsFilingstemp
	where SearchFields is not null and EntityId = @ShipmentId and Tenant = @Tenant
	OPEN DocumentsFilingsCursor FETCH NEXT FROM DocumentsFilingsCursor INTO @SearchFields
	WHILE @@FETCH_STATUS = 0
	BEGIN


		begin

		if(@SearchFields is not null)
		begin 
	    
		if(@DocumentsSearchFields !='')begin set @DocumentsSearchFields = @DocumentsSearchFields + ','  end
		
		set @DocumentsSearchFields = @DocumentsSearchFields + @SearchFields


		end

		end

	FETCH NEXT FROM DocumentsFilingsCursor INTO @SearchFields
	END
	CLOSE DocumentsFilingsCursor
	DEALLOCATE DocumentsFilingsCursor
    
			 update ShipmentComputedFields set DocumentsSearchFields= @DocumentsSearchFields where id = @ShipmentId and Tenant = @Tenant

		end


	FETCH NEXT FROM ShipmentComputedFieldCursor INTO @ShipmentId , @Tenant
	END
	CLOSE ShipmentComputedFieldCursor
	DEALLOCATE ShipmentComputedFieldCursor



	drop table #DocumentsFilingstemp
    

 

	