

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
	
	if( not (@Id = any(select QuoteTemplateId from QuoteTemplateTextCodes where TextCode ='SALEMINMAXPACKAGES' and Tenant = @Tenant)))
	begin
	
	DECLARE  @newIdPackages varchar(100) ;
   	DECLARE  @newIdContainers varchar(100) ;

    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newIdPackages output,@pTableName = 'QuoteTemplateTextCode';
	EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newIdContainers output,@pTableName = 'QuoteTemplateTextCode';

	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area,OriginalEnglishName,OriginalLocalName)
	values(@newIdPackages,@Tenant,'SALEMINMAXPACKAGES','Sale Min/Max','Sale Min/Max',@Id,'Packages','Sale Min/Max','Sale Min/Max')


	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area,OriginalEnglishName,OriginalLocalName)
	values(@newIdContainers,@Tenant,'SALEMINMAXCONTAINERS','Sale Min/Max','Sale Min/Max',@Id,'Containers','Sale Min/Max','Sale Min/Max')


   end


	FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	END
	CLOSE TemplatesCursor
	DEALLOCATE TemplatesCursor
END
