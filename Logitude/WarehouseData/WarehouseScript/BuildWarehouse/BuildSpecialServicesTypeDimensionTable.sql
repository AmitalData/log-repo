
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @EnglishName as nvarchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE SpecialServicesTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Code, EnglishName , LocalName , dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_SpecialServicesTypes.AutomaticLastUpdateDate, dw_SpecialServicesTypes.InActive
	From dw_SpecialServicesTypes
	inner JOIN dw_DWHSettings ON dw_SpecialServicesTypes.Tenant = dw_DWHSettings.Tenant
	OPEN SpecialServicesTypesCursor FETCH NEXT FROM SpecialServicesTypesCursor INTO @Id ,@Code, @EnglishName, @LocalName , @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_SpecialServicesTypesTemp (Id, Code ,[English Name] ,[Local Name], [Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id ,@Code, @EnglishName, @LocalName,  @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive)

	FETCH NEXT FROM SpecialServicesTypesCursor  INTO @Id ,@Code, @EnglishName, @LocalName, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE SpecialServicesTypesCursor
	DEALLOCATE SpecialServicesTypesCursor

