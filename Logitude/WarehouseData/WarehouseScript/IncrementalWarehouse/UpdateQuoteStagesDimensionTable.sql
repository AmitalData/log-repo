
 declare @MaxAutomaticLastUpdateDate as datetime
 declare @LastUpdateDate as datetime

 set @LastUpdateDate = (select top(1) LastUpdateDate from dw_WaterMarks  where TableName = 'QuoteStage' )
 set @MaxAutomaticLastUpdateDate = (select  MAX( AutomaticLastUpdateDate) AutomaticLastUpdateDate from dw_QuoteStages )

 
 if(@MaxAutomaticLastUpdateDate > @LastUpdateDate)

 begin
   declare @Key as varchar(15)
   declare @Id as varchar(15) 
   declare @Name as varchar(40)
   declare @Code as varchar(4)     
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE QuoteStagesCursor CURSOR READ_ONLY
	FOR
    SELECT Id,Name, Code, dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_QuoteStages.AutomaticLastUpdateDate
	From dw_QuoteStages
	inner JOIN dw_DWHSettings ON dw_QuoteStages.Tenant = dw_DWHSettings.Tenant
	where dw_QuoteStages.AutomaticLastUpdateDate > @LastUpdateDate	
	OPEN QuoteStagesCursor FETCH NEXT FROM QuoteStagesCursor INTO   @Id , @Name, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	set @Key = (select Id from DIM_QuoteStages where Id = @Id)
	
	if(@Key is  null) begin     insert into DIM_QuoteStages (Id,[Name], Code, [Source Tenant],[Parent Tenant],[Automatic Last Update Date])
	values(@Id , @Name, @Code, @SourceTenant , @ParentTenant, @AutomaticLastUpdateDate) end
	else begin update  
	DIM_QuoteStages set [Name] =@Name, [Code] =@Code, [Source Tenant] = @SourceTenant , [Parent Tenant] = @ParentTenant, [Automatic Last Update Date] = @AutomaticLastUpdateDate 
	Where Id = @Id end


	FETCH NEXT FROM QuoteStagesCursor INTO   @Id ,@Name, @Code, @SourceTenant ,@ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE QuoteStagesCursor
	DEALLOCATE QuoteStagesCursor


	    update dw_WaterMarks set LastUpdateDate = @MaxAutomaticLastUpdateDate where TableName = 'QuoteStage'
End

