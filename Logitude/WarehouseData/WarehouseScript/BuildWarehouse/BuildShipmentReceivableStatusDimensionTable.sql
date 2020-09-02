

   declare @Code as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentReceivableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_ShipmentReceivableStatuses
	OPEN ShipmentReceivableStatusesCursor FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentReceivableStatusesTemp  (Code,Name) values(@Code,@Name)

	FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code , @Name
		End
	CLOSE ShipmentReceivableStatusesCursor
	DEALLOCATE ShipmentReceivableStatusesCursor
