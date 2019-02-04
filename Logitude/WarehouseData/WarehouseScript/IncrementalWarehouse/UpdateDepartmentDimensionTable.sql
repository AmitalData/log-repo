
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Department' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Departments )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
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
	where dw_Departments.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN DepartmentsCursor FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Departments where Id = @Id)
	
	if(@Key is  null) begin insert into DIM_Departments (Id,Name,[Local Name],[Source Tenant],[Parent Tenant]) values(@Id,@EnglishName,@LocalName,	@SourceTenant , @ParentTenant); end
	else begin update   DIM_Departments set Name =@EnglishName,  [Local Name] =@LocalName , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant Where Id = @Id end


	FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant
		End
	CLOSE DepartmentsCursor
	DEALLOCATE DepartmentsCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'Department'
End

