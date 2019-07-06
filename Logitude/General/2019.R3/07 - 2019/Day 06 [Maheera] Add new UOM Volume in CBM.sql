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

        if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'VCBM')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('VCBM', 'Volume in CBM', 'V in CBM', @MeasurementId, @Tenant, 0, 0, 0, 'VCBM,Volume in CBM,V in CBM', 'Volume in CBM')                            
        end 

        FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END