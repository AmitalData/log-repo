
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'PaymentTerm' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_PaymentTerms )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15)
   declare @Code as varchar(4)
   declare @EnglishName as varchar(40)
   declare @LocalName as nvarchar(40) 
   declare @SourceTenant int
   declare @ParentTenant int 
   declare @AutomaticLastUpdateDate as datetime 

	DECLARE PaymentTermsCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Code, EnglishName, LocalName, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_PaymentTerms.AutomaticLastUpdateDate
	From dw_PaymentTerms
	inner JOIN dw_DWHSettings ON dw_PaymentTerms.Tenant = dw_DWHSettings.Tenant
	where dw_PaymentTerms.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN PaymentTermsCursor FETCH NEXT FROM PaymentTermsCursor INTO   @Id ,@Code, @EnglishName, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_PaymentTerms where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_PaymentTerms (Id, Code ,[English Name],[Local Name], [Source Tenant],[Parent Tenant],[Automatic Last Update Date]) values(@Id ,@Code, @EnglishName, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate) end
	else begin update   DIM_PaymentTerms set [Code] =@Code, [English Name] =@EnglishName, [Local Name] =@LocalName, [Source Tenant] = @SourceTenant, [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate Where Id = @Id end


	FETCH NEXT FROM PaymentTermsCursor INTO   @Id ,@Code, @EnglishName, @LocalName, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE PaymentTermsCursor
	DEALLOCATE PaymentTermsCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'PaymentTerm'
End

