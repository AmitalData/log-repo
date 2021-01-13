
   declare @Id as varchar(15) 
   declare @Name as varchar(60)
   declare @Code as varchar(2) 
   declare @Tenant int
   declare @AutomaticLastUpdateDate as datetime

	DECLARE QuoteClosingReasonsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Name, Code, dw_DWHSettings.Tenant, dw_QuoteClosingReasons.AutomaticLastUpdateDate
	From dw_QuoteClosingReasons
	inner JOIN dw_DWHSettings ON dw_QuoteClosingReasons.Tenant = dw_DWHSettings.Tenant
	OPEN QuoteClosingReasonsCursor FETCH NEXT FROM QuoteClosingReasonsCursor INTO  @Id ,@Name ,@Code,@Tenant, @AutomaticLastUpdateDate
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
    insert into #DIM_QuoteClosingReasonsTemp (Id, Name, Code,[Tenant], [Automatic Last Update Date]) values (@Id ,@Name, @Code, @Tenant, @AutomaticLastUpdateDate)

	FETCH NEXT FROM QuoteClosingReasonsCursor  INTO @Id, @Name, @Code, @Tenant, @AutomaticLastUpdateDate
		End
	CLOSE QuoteClosingReasonsCursor
	DEALLOCATE QuoteClosingReasonsCursor
 