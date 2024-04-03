
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @Name as varchar(60)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE ShipmentSubTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Code, [Name], dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_ShipmentSubTypes.AutomaticLastUpdateDate, dw_ShipmentSubTypes.InActive
	From dw_ShipmentSubTypes
	inner JOIN dw_DWHSettings ON dw_ShipmentSubTypes.Tenant = dw_DWHSettings.Tenant
	OPEN ShipmentSubTypesCursor FETCH NEXT FROM ShipmentSubTypesCursor INTO @Id, @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_ShipmentSubTypesTemp (Id, Code, [Name], [Source Tenant], [Parent Tenant], [Automatic Last Update Date], [InActive]) values(@Id, @Code, @Name,  @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM ShipmentSubTypesCursor  INTO @Id, @Code, @Name, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE ShipmentSubTypesCursor
	DEALLOCATE ShipmentSubTypesCursor

