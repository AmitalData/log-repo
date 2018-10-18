
	
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
	
	 set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = 'CHARGENOTEPACKAGES' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
	
	if(@QuoteTemplateTextCodeId is null)
	   begin

	        EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
            INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'CHARGENOTEPACKAGES', 'Charge Note' , 'Charge Note' , @QuoteTemplateId ,'Packages' ,'Charge Note','Charge Note' );


		    EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
	        INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'CHARGENOTECONTAINERS', 'Charge Note' , 'Charge Note' , @QuoteTemplateId ,'Containers' ,'Charge Note','Charge Note' );

		end

	FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant 

	End
	CLOSE QuoteTemplateCursor
	DEALLOCATE QuoteTemplateCursor
