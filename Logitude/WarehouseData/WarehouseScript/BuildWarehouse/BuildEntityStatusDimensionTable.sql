
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
	OPEN EntityStatusCursor FETCH NEXT FROM EntityStatusCursor INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentStatusesTemp  (Id,Name,Code,[Source Tenant],[Parent Tenant]) values(@Id,@Name,@Code , 	@SourceTenant , @ParentTenant)

	FETCH NEXT FROM EntityStatusCursor  INTO @Id , @Name, @Code, 	@SourceTenant , @ParentTenant
		End
	CLOSE EntityStatusCursor
	DEALLOCATE EntityStatusCursor
