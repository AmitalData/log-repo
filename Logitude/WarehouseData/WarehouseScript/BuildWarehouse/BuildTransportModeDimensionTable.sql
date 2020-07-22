

   declare @Id as varchar(1)
   declare @Name as varchar(10)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_TransportModes
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Id , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TransportModesTemp  (Code,Name,[Automatic Last Update Date]) values(@Id,@Name,@AutomaticLastUpdateDate)

	FETCH NEXT FROM TransportModesCursor INTO @Id , @Name, @AutomaticLastUpdateDate
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor
