

   declare @Id as varchar(1)
   declare @Name as varchar(40)

	DECLARE DirectionsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Directions
	OPEN DirectionsCursor FETCH NEXT FROM DirectionsCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DirectionsTemp (Code,Name) values(@Id,@Name)

	FETCH NEXT FROM DirectionsCursor INTO @Id , @Name
		End
	CLOSE DirectionsCursor
	DEALLOCATE DirectionsCursor
