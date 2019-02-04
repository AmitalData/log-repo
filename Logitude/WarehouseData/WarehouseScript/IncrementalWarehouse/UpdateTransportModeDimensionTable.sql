declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'TransportMode' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_TransportModes )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)

   declare @Id as varchar(1)
   declare @Name as varchar(10)

	DECLARE TransportModesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_TransportModes
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN TransportModesCursor FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_TransportModes where Code = @Id)
	if(@Key is  null) begin  insert into DIM_TransportModes (Code,Name) values(@Id,@Name); end
	else begin update   DIM_TransportModes set Name =@Name Where Code = @Id; end

    

	FETCH NEXT FROM TransportModesCursor INTO @Id , @Name
		End
	CLOSE TransportModesCursor
	DEALLOCATE TransportModesCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'TransportMode'
	End