

declare @Tenant as int
declare @MeasurementId as varchar(15)
declare @PackageTypeId as varchar(15)

BEGIN 
	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR
	SELECT Id
	FROM Tenants
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant        
	WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MeasurementId = (select Id from Measurements where Tenant = @Tenant and Code = '20HC')

        if (@MeasurementId is null)
        begin 
		           
			EXECUTE usp_GetNextTableIdValue @MeasurementId OUTPUT,'Measurement'
            insert into Measurements(Code, Name, ShortName, Id, Tenant, IsContainerMeasurement, IsContainer, InActive, SearchFields, LocalName)
            values('20HC', '20 ft. high cube', '20 ft. high cube', @MeasurementId, @Tenant, 1, 1, 0, '20HC, 20 ft. high cube, 20 ft. high cube', '20 ft. high cube')                            
        end 
		
		if not exists (select Id from PackageTypes where Tenant = @Tenant and Code = '20HC' and MeasurementId = @MeasurementId)
        begin 		           
			EXECUTE usp_GetNextTableIdValue @PackageTypeId OUTPUT,'PackageType'
            insert into PackageTypes
			(
			Id, Tenant, Code, EnglishName, LocalName, IsContainer, InActive, IsAir, IsOcean, IsInland, ContainerSize, TEU, PrintAs, SearchFields, AddedManually, MeasurementId
			)
            values
			(
			@PackageTypeId,
			@Tenant,
			'20HC', 
			'20 ft. high cube', 
			'20 ft. high cube',			 
			1, 
			0, 
			0, 
			1,
			1,			
			20,
			1,
			'20HC',
			'20HC,20 ft. high cube,20 ft. high cube',
			0,
			@MeasurementId
			)                            
        end                    

           FETCH NEXT FROM TenantsCursor INTO @Tenant      
        END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor
END