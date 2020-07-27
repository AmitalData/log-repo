
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'EntityStatus' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentStatuses )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @Code as varchar(4)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   
	DECLARE EntityStatusCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ShipmentStatuses.Id,dw_ShipmentStatuses.Name  ,dw_ShipmentStatuses.Code, dw_ShipmentStatuses.Tenant,dw_DWHSettings.ParentTenant, dw_ShipmentStatuses.AutomaticLastUpdateDate
	From dw_ShipmentStatuses
	inner JOIN dw_DWHSettings ON dw_ShipmentStatuses.Tenant = dw_DWHSettings.Tenant
	where dw_ShipmentStatuses.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN EntityStatusCursor FETCH NEXT FROM EntityStatusCursor INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @Key = (select Id from DIM_ShipmentStatuses where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_ShipmentStatuses (Id,Name,Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values(@Id,@Name,@Code , 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate) end
	else begin update   DIM_ShipmentStatuses set Name =@Name,  Code =@Code , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Id = @Id end
   

	FETCH NEXT FROM EntityStatusCursor  INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE EntityStatusCursor
	DEALLOCATE EntityStatusCursor
	
	   update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'EntityStatus'
End

