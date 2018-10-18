BEGIN;









Delete From QuoteTemplateTextCodes Where TextCode = 'DANGEROUSOFGOODS'


declare @Tenant as int
declare @Id as varchar(15)
	DECLARE TemplatesCursor CURSOR READ_ONLY
	FOR
	SELECT Id,Tenant
	From QuoteTemplates
	OPEN TemplatesCursor FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	if( not (@Id = any(select QuoteTemplateId from QuoteTemplateTextCodes where TextCode ='DANGEROUSGOODS' and Tenant = @Tenant)))
	begin
	
	DECLARE  @newId varchar(100) ;
   
    EXECUTE usp_GetNextTableIdValue  @pLastNumber = @newId output,@pTableName = 'QuoteTemplateTextCode';
    print @newId
	insert into QuoteTemplateTextCodes (Id,Tenant,TextCode,EnglishName,LocalName,QuoteTemplateId,Area)
	values(@newId,@Tenant,'DANGEROUSGOODS','Dangerous Goods','Dangerous Goods',@Id,'QuoteDetails')
		 
	print  @Id print  @Tenant 
   end


	FETCH NEXT FROM TemplatesCursor INTO @Id,@Tenant
	END
	CLOSE TemplatesCursor
	DEALLOCATE TemplatesCursor
END
