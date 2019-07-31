
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int
   

	DECLARE SpecialServicesTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Code, EnglishName , LocalName , dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_SpecialServicesTypes
	inner JOIN dw_DWHSettings ON dw_SpecialServicesTypes.Tenant = dw_DWHSettings.Tenant
	OPEN SpecialServicesTypesCursor FETCH NEXT FROM SpecialServicesTypesCursor INTO @Id ,@Code, @EnglishName, @LocalName , @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_SpecialServicesTypesTemp (Id, Code ,[English Name] ,[Local Name], [Source Tenant],[Parent Tenant]) values(@Id ,@Code, @EnglishName, @LocalName,  @SourceTenant , @ParentTenant)

	FETCH NEXT FROM SpecialServicesTypesCursor  INTO @Id ,@Code, @EnglishName, @LocalName, @SourceTenant , @ParentTenant
		End
	CLOSE SpecialServicesTypesCursor
	DEALLOCATE SpecialServicesTypesCursor

