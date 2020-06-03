declare @Tenant as int
declare @MeasurementId as varchar(15)

BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	WHILE @@FETCH_STATUS = 0
		BEGIN
     
			update Measurements set Name = 'Storage Days x Weight', ShortName = 'Storage Days x Weight', LocalName = 'Storage Days x Weight', SearchFields = 'SCGW,Storage Days x Weight,Storage Days x Weight'
			where  Tenant = @Tenant and Code = 'SCGW'
		
        FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END
