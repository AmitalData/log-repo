

	
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ShipmentSubType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentSubTypes )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @Name as nvarchar(60)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE ShipmentSubTypesCursor CURSOR READ_ONLY
	FOR
    SELECT Id, Code, [Name], dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_ShipmentSubTypes.AutomaticLastUpdateDate, dw_ShipmentSubTypes.InActive
	From dw_ShipmentSubTypes
	inner JOIN dw_DWHSettings ON dw_ShipmentSubTypes.Tenant = dw_DWHSettings.Tenant
	where dw_ShipmentSubTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN ShipmentSubTypesCursor FETCH NEXT FROM ShipmentSubTypesCursor INTO  @Id, @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_ShipmentSubTypes where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_ShipmentSubTypes (Id, Code, [Name], [Source Tenant], [Parent Tenant], [Automatic Last Update Date], [InActive]) values(@Id, @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive) end
	else begin update   DIM_ShipmentSubTypes set [Code] = @Code, [Name] = @Name, [Source Tenant] = @SourceTenant, [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate, [InActive] = @InActive Where Id = @Id end


	FETCH NEXT FROM ShipmentSubTypesCursor INTO  @Id, @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE ShipmentSubTypesCursor
	DEALLOCATE ShipmentSubTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'ShipmentSubType'
End


