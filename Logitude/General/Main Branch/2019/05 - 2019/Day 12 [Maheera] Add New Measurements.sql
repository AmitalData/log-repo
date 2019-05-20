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

        if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'CWKG')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('CWKG', 'Chargeable Weight in Kg', 'CW in Kg', @MeasurementId, @Tenant, 0, 0, 0, 'CWKG,Chargeable Weight in Kg,CW in Kg', 'Chargeable Weight in Kg')                            
        end 
		
		if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'GWKG')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('GWKG', 'Gross Weight in Kg', 'GW in Kg', @MeasurementId, @Tenant, 0, 0, 0, 'GWKG,Gross Weight in Kg,GW in Kg', 'Gross Weight in Kg')                            
        end 

        FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END