declare @DocumentTypeId varchar(15)
DECLARE @Tenant AS INT
declare @DocumentTypeCopyId varchar(15)

	DECLARE DocsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants where id = 0 or id = 1586
	OPEN DocsCursor FETCH NEXT FROM DocsCursor INTO @Tenant 
	WHILE @@FETCH_STATUS = 0
	BEGIN	
	    -- SCMR
		set @DocumentTypeId = (select Id from DocumentTypeCopies where Code = 'SCMR' and Tenant = 4)
		if(@DocumentTypeId is not null)
		begin
			EXECUTE usp_GetNextTableIdValue @DocumentTypeCopyId OUTPUT,'DocumentTypeCopy'    
			insert into DocumentTypeCopies (Id, Tenant, Code, Name, DocumentTypeId, IndexOrder, IsSelectedByDefault, InActive) 
			values(@DocumentTypeCopyId, @Tenant , 'SCMR', 'CMR' ,@DocumentTypeId, 0, 0, 0)
		end

		FETCH NEXT FROM DocsCursor INTO @Tenant
END
CLOSE DocsCursor
DEALLOCATE DocsCursor	





