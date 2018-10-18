
	
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
	
	 set @QuoteTemplateTextCodeId = (select Id from QuoteTemplateTextCodes where TextCode = 'NOTIFYNAME' AND QuoteTemplateId = @QuoteTemplateId AND Tenant = @Tenant )
	
	if(@QuoteTemplateTextCodeId is null)
	   begin

	        EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
            INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'NOTIFYNAME', 'Notify Name' , 'Notify Name' , @QuoteTemplateId ,'QuoteDetails' ,'Notify Name','Notify Name' );

			EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
            INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'NOTIFYADDRESS', 'Notify Address' , 'Notify Address' , @QuoteTemplateId ,'QuoteDetails' ,'Notify Address','Notify Address' );

		    EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'QuoteTemplateTextCode'
            INSERT INTO QuoteTemplateTextCodes VALUES (@NewEntityId, @Tenant , 'NOTIFYCONTACT', 'Notify Contact' , 'Notify Contact' , @QuoteTemplateId ,'QuoteDetails' ,'Notify Contact','Notify Contact' );

		end

	FETCH NEXT FROM QuoteTemplateCursor INTO @QuoteTemplateId , @Tenant 

	End
	CLOSE QuoteTemplateCursor
	DEALLOCATE QuoteTemplateCursor
