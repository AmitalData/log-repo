
   declare @Id as varchar(15) 
   declare @Name as varchar(40)
   declare @Code as varchar(4) 
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE QuoteStagesCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, Code,dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_QuoteStages.AutomaticLastUpdateDate
	From dw_QuoteStages
	inner JOIN dw_DWHSettings ON dw_QuoteStages.Tenant = dw_DWHSettings.Tenant
	OPEN QuoteStagesCursor FETCH NEXT FROM QuoteStagesCursor INTO  @Id, @Name ,@Code, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_QuoteStagesTemp (Id, Name, Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values (@Id ,@Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM QuoteStagesCursor  INTO @Id, @Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE QuoteStagesCursor
	DEALLOCATE QuoteStagesCursor
 