declare @GeneralInvoiceDocumentTypeId varchar(15)
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
	
		set @GeneralInvoiceDocumentTypeId = (select Id from DocumentTypes where Code = '999G' and Tenant = @Tenant)
		
		if(@GeneralInvoiceDocumentTypeId is not null)
		begin

			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'    
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant, '999G1', 'Copy 1' ,@GeneralInvoiceDocumentTypeId, 0, 0, 0)
					
		end		

		FETCH NEXT FROM DocsCursor INTO @Tenant
	END
	CLOSE DocsCursor
	DEALLOCATE DocsCursor