

   declare @Id as varchar(1)
   declare @Name as varchar(10)

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_TransportModes
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TransportModesTemp  (Code,Name) values(@Id,@Name)

	FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor
