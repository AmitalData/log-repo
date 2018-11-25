
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE BranchesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_Branches
	inner JOIN dw_DWHSettings ON dw_Branches.Tenant = dw_DWHSettings.Tenant
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_BranchesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant)

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor

