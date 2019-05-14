

	
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'SpecialServicesType' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_SpecialServicesTypes )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(5)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @SourceTenant int
   declare @ParentTenant int

	DECLARE SpecialServicesTypesCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Code, EnglishName , LocalName , dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_SpecialServicesTypes
	inner JOIN dw_DWHSettings ON dw_SpecialServicesTypes.Tenant = dw_DWHSettings.Tenant
	where dw_SpecialServicesTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN SpecialServicesTypesCursor FETCH NEXT FROM SpecialServicesTypesCursor INTO  @Id ,@Code, @EnglishName, @LocalName,  @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_SpecialServicesTypes where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_SpecialServicesTypes (Id, Code ,[English Name] ,[Local Name], [Source Tenant],[Parent Tenant]) values(@Id ,@Code, @EnglishName, @LocalName,  @SourceTenant , @ParentTenant) end
	else begin update   DIM_SpecialServicesTypes set [Code] =@Code, [English Name] =@EnglishName, [Local Name] =@LocalName , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant Where Id = @Id end


	FETCH NEXT FROM SpecialServicesTypesCursor INTO  @Id ,@Code, @EnglishName, @LocalName, @SourceTenant , @ParentTenant
		End
	CLOSE SpecialServicesTypesCursor
	DEALLOCATE SpecialServicesTypesCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'SpecialServicesType'
End


