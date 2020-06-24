

   declare @Code as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentPayableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_ShipmentPayableStatuses
	OPEN ShipmentPayableStatusesCursor FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentPayableStatusesTemp  (Code,Name) values(@Code,@Name)

	FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name
		End
	CLOSE ShipmentPayableStatusesCursor
	DEALLOCATE ShipmentPayableStatusesCursor
