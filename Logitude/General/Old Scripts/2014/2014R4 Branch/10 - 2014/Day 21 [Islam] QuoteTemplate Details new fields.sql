 
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
	
	if( not (@Id = any(select QuoteTemplateId from QuoteTemplateTextCodes where TextCode ='CHARGEABLEWEIGHT' and Tenant = @Tenant)))
	begin
	
	DECLARE  @newId varchar(100) ;
   
    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'CHARGEABLEWEIGHT','Chargeable Weight','Chargeable Weight',@Id,'QuoteDetails')

    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'GROSSWEIGHT','Gross Weight','Gross Weight',@Id,'QuoteDetails')

    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'VOLUME','Volume','Volume',@Id,'QuoteDetails')

	EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'VOLUMETRICWEIGHT','Volumetric Weight','Volumetric Weight',@Id,'QuoteDetails')

	EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'NUMBEROFPACKAGES','Number Of Packages','Number Of Packages',@Id,'QuoteDetails')

	EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'NUMBEROFCONTAINERS','Number Of Containers','Number Of Containers',@Id,'QuoteDetails')


	 
	print  @Id print  @Tenant 
   end


	FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	END
	CLOSE TemplatesCursor
	DEALLOCATE TemplatesCursor
END


