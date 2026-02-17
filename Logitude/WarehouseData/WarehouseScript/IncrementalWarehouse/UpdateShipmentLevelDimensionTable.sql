
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentLevel' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Levels )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)
   declare @Code as varchar(1)
   declare @Name as varchar(40)

	DECLARE ShipmentLevelsCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_Levels
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentLevelsCursor FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_Levels where Code = @Code)
	if(@Key is  null) begin  insert into DIM_Levels  (Code,Name) values(@Code,@Name); end
	else begin update   DIM_Levels set Name =@Name Where Code = @Code; end
   

	FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name
		End
	CLOSE ShipmentLevelsCursor
	DEALLOCATE ShipmentLevelsCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ShipmentLevel'
	End