
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'OBLType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_OBLTypes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)
   declare @Code as varchar(1)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE OBLTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_OBLTypes
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN OBLTypesCursor FETCH NEXT FROM OBLTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_OBLTypes where Code = @Code)
	if(@Key is  null) begin  insert into DIM_OBLTypes  (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_OBLTypes set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end
   

	FETCH NEXT FROM OBLTypesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE OBLTypesCursor
	DEALLOCATE OBLTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'OBLType'
	End