
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Direction' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Directions )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)
   declare @Code as varchar(1)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE DirectionsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, AutomaticLastUpdateDate
	From dw_Directions
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN DirectionsCursor FETCH NEXT FROM DirectionsCursor INTO @Code , @Name , @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Code from DIM_Directions where Code = @Code)
	if(@Key is  null) begin insert into DIM_Directions (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_Directions set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end
		
	FETCH NEXT FROM DirectionsCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE DirectionsCursor
	DEALLOCATE DirectionsCursor

    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Direction'


	end

