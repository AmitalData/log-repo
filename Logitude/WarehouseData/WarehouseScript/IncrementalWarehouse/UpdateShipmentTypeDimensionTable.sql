 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Types )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(4)
   declare @Code as varchar(4)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_Types
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentTypesCursor FETCH NEXT FROM ShipmentTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_Types where Code = @Code)
	if(@Key is  null) begin  insert into DIM_Types (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_Types set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end
   
    
	FETCH NEXT FROM ShipmentTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentTypesCursor
	DEALLOCATE ShipmentTypesCursor
	
	
	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ShipmentType'
	End

