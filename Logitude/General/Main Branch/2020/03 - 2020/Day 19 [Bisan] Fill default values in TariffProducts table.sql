declare @Tenant as int
declare @NewEntityId as varchar(15)
BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

			EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'TariffProduct'
			INSERT INTO TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
			VALUES (@NewEntityId, @Tenant , 'GEN', 'General', 'General' , 0 , 'GEN, General');
		
			EXECUTE usp_GetNextTableIdValue @NewEntityId OUTPUT,'TariffProduct'
			INSERT INTO TariffProducts(Id, Tenant, Code, Name, LocalName, Inactive, SearchFields)
			VALUES (@NewEntityId, @Tenant , 'DNG', 'Dangerous Goods', 'Dangerous Goods' , 0 , 'DNG, Dangerous Goods');

		FETCH NEXT FROM TenantsCursor INTO  @Tenant
		END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END