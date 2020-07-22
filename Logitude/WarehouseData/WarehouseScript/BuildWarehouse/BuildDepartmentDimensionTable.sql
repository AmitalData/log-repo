

   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE DepartmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Departments.Id, dw_Departments.EnglishName , dw_Departments.LocalName , dw_Departments.Tenant, dw_DWHSettings.ParentTenant, dw_Departments.AutomaticLastUpdateDate
	From dw_Departments
	inner JOIN dw_DWHSettings ON dw_Departments.Tenant = dw_DWHSettings.Tenant
	OPEN DepartmentsCursor FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_DepartmentsTemp (Id,Name,[Local Name],[Source Tenant],[Parent Tenant],[Automatic Last Update Date]) values(@Id,@EnglishName,@LocalName,	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE DepartmentsCursor
	DEALLOCATE DepartmentsCursor
	
