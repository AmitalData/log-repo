

   declare @Id as varchar(1)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE DirectionsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_Directions
	OPEN DirectionsCursor FETCH NEXT FROM DirectionsCursor INTO @Id , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DirectionsTemp (Code,Name,[Automatic Last Update Date]) values(@Id,@Name, @AutomaticLastUpdateDate)

	FETCH NEXT FROM DirectionsCursor INTO @Id , @Name, @AutomaticLastUpdateDate
		End
	CLOSE DirectionsCursor
	DEALLOCATE DirectionsCursor
