
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE BranchesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Branches.AutomaticLastUpdateDate, dw_Branches.InActive
	From dw_Branches
	inner JOIN dw_DWHSettings ON dw_Branches.Tenant = dw_DWHSettings.Tenant
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_BranchesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date],[InActive]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate,@InActive)

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate,@InActive
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor

