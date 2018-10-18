
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

        if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'PRFR')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('PRFR', 'Percent of Freight', 'Percent of Freight', @MeasurementId, @Tenant, 0, 0, 0, 'PRFR,Percent of Freight,Percent of Freight', 'Percent of Freight')                            
        end 
		
		if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'PRVL')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('PRVL', 'Percent of Value', 'Percent of Value', @MeasurementId, @Tenant, 0, 0, 0, 'PRVL,Percent of Value,Percent of Value', 'Percent of Value')                            
        end                    

           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END