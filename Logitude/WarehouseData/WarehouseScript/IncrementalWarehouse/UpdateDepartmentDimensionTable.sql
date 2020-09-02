
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Department' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Departments )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime
   declare @InActive as bit

	DECLARE DepartmentsCursor CURSOR READ_ONLY
	FOR
	SELECT dw_Departments.Id, dw_Departments.EnglishName , dw_Departments.LocalName , dw_Departments.Tenant, dw_DWHSettings.ParentTenant, dw_Departments.AutomaticLastUpdateDate, dw_Departments.InActive
	From dw_Departments
	inner JOIN dw_DWHSettings ON dw_Departments.Tenant = dw_DWHSettings.Tenant
	where dw_Departments.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN DepartmentsCursor FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Departments where Id = @Id)
	
	if(@Key is  null) begin insert into DIM_Departments (Id,Name,[Local Name],[Source Tenant],[Parent Tenant], [Automatic Last Update Date], [InActive]) values(@Id,@EnglishName,@LocalName,	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive); end
	else begin update   DIM_Departments set Name =@EnglishName,  [Local Name] =@LocalName , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate, [InActive] = @InActive Where Id = @Id end


	FETCH NEXT FROM DepartmentsCursor INTO @Id , @EnglishName, @LocalName, 	@SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE DepartmentsCursor
	DEALLOCATE DepartmentsCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Department'
End

