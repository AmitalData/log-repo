 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'Branch' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_Branches )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin


   declare @Key as varchar(15)
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
	where dw_Branches.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN BranchesCursor FETCH NEXT FROM BranchesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_Branches where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_Branches (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate, @InActive) end
	else begin update   DIM_Branches set Name =@EnglishName,  [Local Name] =@LocalName ,  Code = @Code , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate,[InActive] = @InActive where Id = @Id; end

	FETCH NEXT FROM BranchesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE BranchesCursor
	DEALLOCATE BranchesCursor
	

		update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'Branch'
	End


