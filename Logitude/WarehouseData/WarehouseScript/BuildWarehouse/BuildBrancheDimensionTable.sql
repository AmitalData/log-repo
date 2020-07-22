
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @Code as varchar(10)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE BranchesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_Branches.AutomaticLastUpdateDate
	From dw_Branches
	inner JOIN dw_DWHSettings ON dw_Branches.Tenant = dw_DWHSettings.Tenant
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_BranchesTemp (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor

