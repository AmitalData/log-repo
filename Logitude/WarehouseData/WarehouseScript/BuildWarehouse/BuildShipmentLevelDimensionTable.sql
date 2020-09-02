
   declare @Code as varchar(1)
   declare @Name as varchar(40)

	DECLARE ShipmentLevelsCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_Levels
	OPEN ShipmentLevelsCursor FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_LevelsTemp  (Code,Name) values(@Code,@Name)

	FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
		End
	CLOSE ShipmentLevelsCursor
	DEALLOCATE ShipmentLevelsCursor
