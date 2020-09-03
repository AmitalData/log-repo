
   declare @Code as varchar(4)
   declare @Name as varchar(25)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE OBLTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_OBLTypes
	OPEN OBLTypesCursor FETCH NEXT FROM OBLTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_OBLTypesTemp  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name, @AutomaticLastUpdateDate)

	FETCH NEXT FROM OBLTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE OBLTypesCursor
	DEALLOCATE OBLTypesCursor
