declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentType' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Types )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(4)
   declare @Code as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Types
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentTypesCursor FETCH NEXT FROM ShipmentTypesCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_Types where Code = @Code)
	if(@Key is  null) begin  insert into DIM_Types (Code,Name) values(@Code,@Name); end
	else begin update   DIM_Types set Name =@Name Where Code = @Code; end
   
    
	FETCH NEXT FROM ShipmentTypesCursor INTO @Code , @Name
		End
	CLOSE ShipmentTypesCursor
	DEALLOCATE ShipmentTypesCursor
	
	
	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ShipmentType'
	End

