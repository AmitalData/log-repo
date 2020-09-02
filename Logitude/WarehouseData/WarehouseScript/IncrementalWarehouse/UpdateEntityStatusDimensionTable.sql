
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'EntityStatus' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ShipmentStatuses )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Name as varchar(40)
   declare @Code as varchar(4)
   declare @SourceTenant int
   declare @ParentTenant int
   
   
	DECLARE EntityStatusCursor CURSOR READ_ONLY
	FOR
	SELECT dw_ShipmentStatuses.Id,dw_ShipmentStatuses.Name  ,dw_ShipmentStatuses.Code, dw_ShipmentStatuses.Tenant,dw_DWHSettings.ParentTenant
	From dw_ShipmentStatuses
	inner JOIN dw_DWHSettings ON dw_ShipmentStatuses.Tenant = dw_DWHSettings.Tenant
	where dw_ShipmentStatuses.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN EntityStatusCursor FETCH NEXT FROM EntityStatusCursor INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @Key = (select Id from DIM_ShipmentStatuses where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_ShipmentStatuses (Id,Name,Code,[Source Tenant],[Parent Tenant]) values(@Id,@Name,@Code , 	@SourceTenant , @ParentTenant) end
	else begin update   DIM_ShipmentStatuses set Name =@Name,  Code =@Code , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant Where Id = @Id end
   

	FETCH NEXT FROM EntityStatusCursor  INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
		End
	CLOSE EntityStatusCursor
	DEALLOCATE EntityStatusCursor
	
	   update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'EntityStatus'
End

