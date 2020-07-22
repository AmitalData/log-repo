
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentLevel' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Levels )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)
   declare @Code as varchar(1)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentLevelsCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_Levels
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentLevelsCursor FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_Levels where Code = @Code)
	if(@Key is  null) begin  insert into DIM_Levels  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_Levels set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end
   

	FETCH NEXT FROM ShipmentLevelsCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentLevelsCursor
	DEALLOCATE ShipmentLevelsCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ShipmentLevel'
	End