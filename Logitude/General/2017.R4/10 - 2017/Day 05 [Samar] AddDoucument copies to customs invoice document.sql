
declare @CustomsInvoiceDocumentTypeId varchar(15)
DECLARE @Tenant AS INT
declare @DocumentTypeCopyId varchar(15)

	DECLARE DocsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants	
	OPEN DocsCursor FETCH NEXT FROM DocsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @CustomsInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999CI' and Tenant = @Tenant)
		
		if(@CustomsInvoiceDocumentTypeId is not null)
		begin

			update DocumentTypeCopies set Name = 'Original', Code = '999CI' where Tenant = @Tenant and DocumentTypeId = @CustomsInvoiceDocumentTypeId

			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'    
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999CI1', 'Copy 1' ,@CustomsInvoiceDocumentTypeId, 0, 0, 0)
			
			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999CI2', 'Copy 2' ,@CustomsInvoiceDocumentTypeId, 0, 0, 0)

			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999CI3', 'Electronic Copy' ,@CustomsInvoiceDocumentTypeId, 0, 0, 0)

		end

		FETCH NEXT FROM DocsCursor INTO @Tenant
	END
	CLOSE DocsCursor
	DEALLOCATE DocsCursor	





