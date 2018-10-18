
declare @ConsolidationInvoiceDocumentTypeId varchar(15)
declare @ManifestInvoiceDocumentTypeId varchar(15)
DECLARE @Tenant AS INT
declare @DocumentTypeCopyId varchar(15)

	DECLARE DocsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants	
	OPEN DocsCursor FETCH NEXT FROM DocsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
	BEGIN
	
		set @ConsolidationInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999C' and Tenant = @Tenant)
		set @ManifestInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999M' and Tenant = @Tenant)
		
		if(@ConsolidationInvoiceDocumentTypeId is not null)
		begin

			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'    
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999C1', 'Copy 1' ,@ConsolidationInvoiceDocumentTypeId, 0, 0, 0)
			
			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999C2', 'Copy 2' ,@ConsolidationInvoiceDocumentTypeId, 0, 0, 0)
			
		end

		if(@ManifestInvoiceDocumentTypeId is not null)
		begin

			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'    
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999M1', 'Copy 1' ,@ManifestInvoiceDocumentTypeId, 0, 0, 0)
			
			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999M2', 'Copy 2' ,@ManifestInvoiceDocumentTypeId, 0, 0, 0)
			
		end

		FETCH NEXT FROM DocsCursor INTO @Tenant
	END
	CLOSE DocsCursor
	DEALLOCATE DocsCursor