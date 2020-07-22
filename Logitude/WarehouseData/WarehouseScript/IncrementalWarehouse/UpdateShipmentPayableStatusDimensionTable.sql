declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentPayableStatus' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentPayableStatuses )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin

   declare @Key as varchar(1)

   declare @Code as varchar(4)
   declare @Name as varchar(40)
   declare @AutomaticLastUpdateDate as datetime

	DECLARE ShipmentPayableStatusesCursor CURSOR READ_ONLY
	FOR
	SELECT Code, Name, AutomaticLastUpdateDate
	From dw_ShipmentPayableStatuses
	where AutomaticLastUpdateDate > @LastUpdateDate
	OPEN ShipmentPayableStatusesCursor FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Code from DIM_ShipmentPayableStatuses where Code = @Code)
	if(@Key is  null) begin  insert into DIM_ShipmentPayableStatuses (Code,Name,[Automatic Last Update Date]) values(@Code,@Name,@AutomaticLastUpdateDate); end
	else begin update   DIM_ShipmentPayableStatuses set Name =@Name, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Code = @Code; end

    

	FETCH NEXT FROM ShipmentPayableStatusesCursor INTO @Code , @Name, @AutomaticLastUpdateDate
		End
	CLOSE ShipmentPayableStatusesCursor
	DEALLOCATE ShipmentPayableStatusesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ShipmentPayableStatus'
	End