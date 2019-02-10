
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Direction' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Directions )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)
   declare @Code as varchar(1)
   declare @Name as varchar(40)

	DECLARE DirectionsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name
	From dw_Directions
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN DirectionsCursor FETCH NEXT FROM DirectionsCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN

	set @Key = (select Code from DIM_Directions where Code = @Code)
	if(@Key is  null) begin insert into DIM_Directions (Code,Name) values(@Code,@Name); end
	else begin update   DIM_Directions set Name =@Name Where Code = @Code; end
		
	FETCH NEXT FROM DirectionsCursor INTO @Code , @Name
		End
	CLOSE DirectionsCursor
	DEALLOCATE DirectionsCursor

    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Direction'


	end

