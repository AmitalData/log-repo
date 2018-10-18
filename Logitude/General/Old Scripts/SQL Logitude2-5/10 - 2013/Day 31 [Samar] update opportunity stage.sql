DECLARE @Tenant AS INT
BEGIN;

	DECLARE TenantCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants	 
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		update Opportunities 
		set StageId = (select Id from Stages where Code = 'CLS' and Tenant = @Tenant)
		where StageId = (select Id from Stages where Code = 'CLC' and Tenant = @Tenant)	
			
	FETCH NEXT FROM TenantCursor INTO @Tenant
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor	
END
go