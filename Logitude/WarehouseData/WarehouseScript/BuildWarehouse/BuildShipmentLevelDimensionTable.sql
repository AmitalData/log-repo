
   declare @Code as varchar(1)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentLevelsCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_Levels
	OPEN ShipmentLevelsCursor FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_LevelsTemp  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name, @AutomaticLastUpdateDate)

	FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentLevelsCursor
	DEALLOCATE ShipmentLevelsCursor
