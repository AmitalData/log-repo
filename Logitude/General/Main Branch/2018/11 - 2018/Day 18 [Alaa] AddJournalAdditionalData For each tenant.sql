

declare @Tenant as int


BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN

				INSERT INTO journaladditionaldatas (journalId, Tenant) SELECT Id, tenant FROM journals where Tenant = @Tenant


				

			FETCH NEXT FROM TenantsCursor INTO @Tenant
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END
