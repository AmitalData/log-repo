 
BEGIN;

declare @Tenant as int
declare @Id as varchar(15)
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	From Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	if(not (@Id = any(select Id from DocumentTypes where Code='QUOTE' and Tenant = @Tenant)))
	begin
	declare @objectTableId varchar(15)
	set @objectTableId = (select id from ObjectTables where name = 'Quote')

	declare @documentTypeId varchar(15)
	EXECUTE usp_GetNextTableIdValue @documentTypeId OUTPUT,'DocumentType'
	insert into DocumentTypes
	(Id,Tenant,Name,Code,IsAir,IsOcean,IsInland,IsDocOut,IsDocIn,IsMaster,IsHouse,IsAgentView,IsCustomerView,IsDirect,InActive,ObjectTableId,[Subject],TemplateFormatCode,DocumentTypeDefaultEditorTool,SearchFields,IsReadOnly)
    values(@documentTypeId,@Tenant,'Quotation Document','QUOTE',1,1,1,0,0,0,0,0,0,0,0,@objectTableId,'Quotation Document','P','R','QUOTE,Quotation Document',1)
    
	print @Tenant 
   end
	FETCH NEXT FROM TenantsCursor INTO @Tenant
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END

