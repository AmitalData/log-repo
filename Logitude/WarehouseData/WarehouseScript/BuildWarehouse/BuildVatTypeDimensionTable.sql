
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE VatTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_VatTypes.AutomaticLastUpdateDate, dw_VatTypes.InActive
	From dw_VatTypes
	inner JOIN dw_DWHSettings ON dw_VatTypes.Tenant = dw_DWHSettings.Tenant
	OPEN VatTypesCursor FETCH NEXT FROM VatTypesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_VatTypesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date],[InActive]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate,@InActive)

	FETCH NEXT FROM VatTypesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate,@InActive
		End
	CLOSE VatTypesCursor
	DEALLOCATE VatTypesCursor


