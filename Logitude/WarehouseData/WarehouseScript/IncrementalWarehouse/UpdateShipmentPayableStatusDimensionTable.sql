declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentPayableStatus' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentPayableStatuses )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)

   declare @Code as varchar(4)
   declare @Name as varchar(40)

	DECLARE ShipmentPayableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name
	From dw_ShipmentPayableStatuses
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentPayableStatusesCursor FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_ShipmentPayableStatuses where Code = @Code)
	if(@Key is  null) begin  insert into DIM_ShipmentPayableStatuses (Code,Name) values(@Code,@Name); end
	else begin update   DIM_ShipmentPayableStatuses set Name =@Name Where Code = @Code; end

    

	FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name
		End
	CLOSE ShipmentPayableStatusesCursor
	DEALLOCATE ShipmentPayableStatusesCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ShipmentPayableStatus'
	End