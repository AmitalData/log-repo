
   declare @Id as varchar(15) 
   declare @Name as varchar(40)
   declare @Code as varchar(4) 
   declare @SourceTenant int
   declare @ParentTenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE QuoteClosingReasonsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, Code,dw_DWHSettings.Tenant, dw_DWHSettings.ParentTenant, dw_QuoteClosingReasons.AutomaticLastUpdateDate
	From dw_QuoteClosingReasons
	inner JOIN dw_DWHSettings ON dw_QuoteClosingReasons.Tenant = dw_DWHSettings.Tenant
	OPEN QuoteClosingReasonsCursor FETCH NEXT FROM QuoteClosingReasonsCursor INTO  @Id ,@Name ,@Code, @SourceTenant, @ParentTenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_QuoteClosingReasonsTemp (Id, Name, Code,[Source Tenant],[Parent Tenant], [Automatic Last Update Date]) values (@Id ,@Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM QuoteClosingReasonsCursor  INTO @Id, @Name, @Code, @SourceTenant,@ParentTenant, @AutomaticLastUpdateDate
		End
	CLOSE QuoteClosingReasonsCursor
	DEALLOCATE QuoteClosingReasonsCursor
 