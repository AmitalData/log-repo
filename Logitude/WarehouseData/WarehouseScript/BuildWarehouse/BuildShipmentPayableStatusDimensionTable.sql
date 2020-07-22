

   declare @Code as varchar(4)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentPayableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_ShipmentPayableStatuses
	OPEN ShipmentPayableStatusesCursor FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code, @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentPayableStatusesTemp  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate)

	FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentPayableStatusesCursor
	DEALLOCATE ShipmentPayableStatusesCursor
