
BEGIN;

declare @Tenant as int
declare @Id as varchar(15)
	DECLARE TemplatesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant
	From QuoteTemplates
	OPEN TemplatesCursor FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	if( not (@Id = any(select QuoteTemplateId from QuoteTemplateTextCodes where TextCode ='TRANSITTIME' and Tenant = @Tenant)))
	begin
	
	DECLARE  @newId varchar(100) ;
   
    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area,OriginalEnglishName,OriginalLocalName)
	values(@newId,@Tenant,'TRANSITTIME','Transit Time','Transit Time',@Id,'QuoteDetails','Transit Time','Transit Time')

   end


	FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	END
	CLOSE TemplatesCursor
	DEALLOCATE TemplatesCursor
END
