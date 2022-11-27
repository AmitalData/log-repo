 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'VatType' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_VatTypes )

 
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
	DECLARE VatTypesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, EnglishName , LocalName ,Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_VatTypes.AutomaticLastUpdateDate, dw_VatTypes.InActive
	From dw_VatTypes
	inner JOIN dw_DWHSettings ON dw_VatTypes.Tenant = dw_DWHSettings.Tenant
	where dw_VatTypes.AutomaticLastUpdateDate > @LastUpdateDate
	OPEN VatTypesCursor FETCH NEXT FROM VatTypesCursor INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_VatTypes where Id = @Id)
	
	if(@Key is  null) begin  insert into DIM_VatTypes (Id,Name,[Local Name],Code,[Source Tenant],[Parent Tenant],[Automatic Last Update Date],[InActive]) values(@Id,@EnglishName,@LocalName,@Code , @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate, @InActive) end
	else begin update   DIM_VatTypes set Name =@EnglishName,  [Local Name] =@LocalName ,  Code = @Code , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate,[InActive] = @InActive where Id = @Id; end

	FETCH NEXT FROM VatTypesCursor  INTO @Id , @EnglishName, @LocalName, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate, @InActive
		End
	CLOSE VatTypesCursor
	DEALLOCATE VatTypesCursor
		update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'VatType'
	End


