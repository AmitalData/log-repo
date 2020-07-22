

   declare @Id as varchar(4)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_Types
	where Id !='-1'
	OPEN ShipmentTypesCursor FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TypesTemp  (Code,Name,[Automatic Last Update Date]) values(@Id,@Name,@AutomaticLastUpdateDate)

	FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentTypesCursor
	DEALLOCATE ShipmentTypesCursor
