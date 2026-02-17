

   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE DepartmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Departments.Id, dw_Departments.EnglishName , dw_Departments.LocalName , dw_Departments.Tenant, dw_DWHSettings.ParentTenant
	From dw_Departments
	inner JOIN dw_DWHSettings ON dw_Departments.Tenant = dw_DWHSettings.Tenant
	OPEN DepartmentsCursor FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DepartmentsTemp (Id,Name,[Local Name],[Source Tenant],[Parent Tenant]) values(@Id,@EnglishName,@LocalName,	@SourceTenant , @ParentTenant)

	FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
		End
	CLOSE DepartmentsCursor
	DEALLOCATE DepartmentsCursor
	
