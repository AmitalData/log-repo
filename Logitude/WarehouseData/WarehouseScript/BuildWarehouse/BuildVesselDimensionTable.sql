
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Notes as varchar(250)
   declare @IMOCode as varchar(10)

   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE VesselsCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Code, EnglishName , LocalName , Notes, IMOCode, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Vessels.AutomaticLastUpdateDate, dw_Vessels.InActive
	From dw_Vessels
	inner JOIN dw_DWHSettings ON dw_Vessels.Tenant = dw_DWHSettings.Tenant
	OPEN VesselsCursor FETCH NEXT FROM VesselsCursor INTO @Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_VesselsTemp (Id, Code ,[English Name] ,[Local Name],[Notes],[IMO Code], [Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM VesselsCursor  INTO @Id ,@Code, @EnglishName, @LocalName, @Notes, @IMOCode , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE VesselsCursor
	DEALLOCATE VesselsCursor

