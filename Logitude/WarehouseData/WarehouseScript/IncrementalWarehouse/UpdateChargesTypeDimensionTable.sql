
 declare @AutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'ChargesType' )
 set @AutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_ChargesTypes )

 
 if(@AutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(15)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40)
   declare @ChargeGroupCode as varchar(5)
  declare @SourceTenant int
   declare @ParentTenant int

	DECLARE ChargesTypeCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Code, EnglishName , LocalName , ChargesGroupCode, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant
	From dw_ChargesTypes
	inner JOIN dw_DWHSettings ON dw_ChargesTypes.Tenant = dw_DWHSettings.Tenant
	where dw_ChargesTypes.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN ChargesTypeCursor FETCH NEXT FROM ChargesTypeCursor INTO   @Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_ChargesTypes where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_ChargesTypes (Id, Code ,[English Name] ,[Local Name],[Charge Group Code], [Source Tenant],[Parent Tenant]) values(@Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant) end
	else begin update   DIM_ChargesTypes set [Code] =@Code, [English Name] =@EnglishName, [Local Name] =@LocalName , [Charge Group Code] = @ChargeGroupCode , [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant Where Id = @Id end


	FETCH NEXT FROM ChargesTypeCursor INTO   @Id ,@Code, @EnglishName, @LocalName, @ChargeGroupCode , @SourceTenant , @ParentTenant
		End
	CLOSE ChargesTypeCursor
	DEALLOCATE ChargesTypeCursor


	    update dw_WaterMarks set LastUpdateDate = @AutomaticLastUpdateDate where TableName = 'ChargesType'
End

