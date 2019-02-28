
	
declare @QuoteTemplateId as varchar(15)
declare @QuoteTemplateTextCodeId as varchar(15)
declare @Tenant as int
declare @NewEntityId as varchar(15)

	DECLARE QuoteTemplateCursor CURSOR READ_ONLY
	FOR
	SELECT Id , Tenant
	From QuoteTemplates
	OPEN QuoteTemplateCursor FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

	 set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = 'MOVETYPE' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
	
	if(@QuoteTemplateTextCodeId is null)
	   begin

	        EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
            INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'MOVETYPE', 'Move Type' , 'Move Type' , @QuoteTemplateId ,'QuoteDetails' ,'Move Type','Move Type' );
		end

	FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant 

	End
	CLOSE QuoteTemplateCursor
	DEALLOCATE QuoteTemplateCursor
