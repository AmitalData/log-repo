
   declare @QuoteId as varchar(15)
   declare @QuoteTemplateId as varchar(15)
   declare @Tenant as int
   declare @QuotationSections as varchar(500)
   set @QuotationSections = ''
   declare @QuoteTemplateSectionId as varchar(15)

	DECLARE QuotesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,QuoteTemplateId , Tenant
	From Quotes
	where QuoteTemplateId is not null and QuoteTemplateId !='' and (QuotationSections is  null or QuotationSections ='' )
	OPEN QuotesCursor FETCH NEXT FROM QuotesCursor INTO @QuoteId,  @QuoteTemplateId , @Tenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN

    DECLARE QuoteTemplateSections CURSOR READ_ONLY
	FOR
	SELECT Id
	From QuoteTemplateSections
	where QuoteTemplateId  =  @QuoteTemplateId and Tenant = @Tenant and IsCancel = 0
	OPEN QuoteTemplateSections FETCH NEXT FROM QuoteTemplateSections INTO @QuoteTemplateSectionId
	WHILE @@FETCH_STATUS = 0
	BEGIN

	
	set @QuotationSections =(@QuotationSections + @QuoteTemplateSectionId + ',')
	
	FETCH NEXT FROM QuoteTemplateSections INTO @QuoteTemplateSectionId 

	End
	CLOSE QuoteTemplateSections
	DEALLOCATE QuoteTemplateSections

	set @QuotationSections = STUFF(@QuotationSections,DATALENGTH(@QuotationSections), 1, '')
	update Quotes set QuotationSections = @QuotationSections where id = @QuoteId and Tenant = @Tenant
	set @QuotationSections = ''
	FETCH NEXT FROM QuotesCursor INTO  @QuoteId,  @QuoteTemplateId , @Tenant 

	End
	CLOSE QuotesCursor
	DEALLOCATE QuotesCursor
