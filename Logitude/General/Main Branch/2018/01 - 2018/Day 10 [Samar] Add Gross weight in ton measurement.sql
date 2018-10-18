
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

        if not exists (select Id from Measurements where Tenant = @Tenant and Code = 'GWTN')
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('GWTN', 'Gross Weight in Ton', 'Gr Weight in Ton', @MeasurementId, @Tenant, 0, 0, 0, 'GWTN,Gross Weight in Ton,Gr Weight in Ton', 'Gross Weight in Ton')                            
        end 
		
           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END