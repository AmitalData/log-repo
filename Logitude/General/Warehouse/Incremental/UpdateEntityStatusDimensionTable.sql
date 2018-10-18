
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
	SELECT Id, Name  ,Code, Tenant, Tenant
	From dw_ShipmentStatuses
	where AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN EntityStatusCursor FETCH NEXT FROM EntityStatusCursor INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	set @Key = (select Id from DIM_ShipmentStatuses where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_ShipmentStatuses values(@Id,@Name,@Code , 	@SourceTenant , @ParentTenant); end
	else begin update   DIM_ShipmentStatuses set Name =@Name,  Code =@Code , SourceTenant = @SourceTenant , ParentTenant = @ParentTenant Where Id = @Id end
   

	FETCH NEXT FROM EntityStatusCursor  INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
		End
	CLOSE EntityStatusCursor
	DEALLOCATE EntityStatusCursor
	
	   update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'EntityStatus'
End

