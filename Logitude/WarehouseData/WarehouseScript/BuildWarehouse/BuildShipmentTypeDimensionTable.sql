

   declare @Id as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Types
	where Id !='-1'
	OPEN ShipmentTypesCursor FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_TypesTemp  (Code,Name) values(@Id,@Name)

	FETCH NEXT FROM ShipmentTypesCursor INTO @Id , @Name
		End
	CLOSE ShipmentTypesCursor
	DEALLOCATE ShipmentTypesCursor
