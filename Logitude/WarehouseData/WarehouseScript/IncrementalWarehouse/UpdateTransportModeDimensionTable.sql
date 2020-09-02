 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'TransportMode' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_TransportModes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)

   declare @Code as varchar(1)
   declare @Name as varchar(10)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_TransportModes
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_TransportModes where Code = @Code)
	if(@Key is  null) begin  insert into DIM_TransportModes (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_TransportModes set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end

    

	FETCH NEXT FROM TransportModesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'TransportMode'
	End