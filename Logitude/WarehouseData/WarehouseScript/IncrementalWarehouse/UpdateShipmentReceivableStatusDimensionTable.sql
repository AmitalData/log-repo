declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentReceivableStatus' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentReceivableStatuses )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)

   declare @Code as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentReceivableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_ShipmentReceivableStatuses
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentReceivableStatusesCursor FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_ShipmentReceivableStatuses where Code = @Code)
	if(@Key is  null) begin  insert into DIM_ShipmentReceivableStatuses (Code,Name) values(@Code,@Name); end
	else begin update   DIM_ShipmentReceivableStatuses set Name =@Name Where Code = @Code; end

    

	FETCH NEXT FROM ShipmentReceivableStatusesCursor INTO @Code , @Name
		End
	CLOSE ShipmentReceivableStatusesCursor
	DEALLOCATE ShipmentReceivableStatusesCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ShipmentReceivableStatus'
	End