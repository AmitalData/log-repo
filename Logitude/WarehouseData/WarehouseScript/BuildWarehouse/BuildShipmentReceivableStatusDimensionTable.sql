

   declare @Code as varchar(4)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentReceivableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_ShipmentReceivableStatuses
	OPEN ShipmentReceivableStatusesCursor FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code, @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentReceivableStatusesTemp  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate)

	FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code, @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentReceivableStatusesCursor
	DEALLOCATE ShipmentReceivableStatusesCursor
