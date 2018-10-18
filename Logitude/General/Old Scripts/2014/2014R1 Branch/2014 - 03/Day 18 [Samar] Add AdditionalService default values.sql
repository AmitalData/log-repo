          
declare @Tenant as int
declare @ServiceId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from AdditionalServices where Tenant = @Tenant and Code = 'OS')
			begin
			
					EXECUTE usp_GetNextTableIdValue @ServiceId OUTPUT,'AdditionalService'
					insert into AdditionalServices(Id, Tenant, Code, Name, SearchFields, InActive)
					values(@ServiceId, @Tenant, 'OS', 'On Site', 'OS,On Site', 0)
		    end

			if not exists (select Id from AdditionalServices where Tenant = @Tenant and Code = 'WH')
			begin
					EXECUTE usp_GetNextTableIdValue @ServiceId OUTPUT,'AdditionalService'
					insert into AdditionalServices(Id, Tenant, Code, Name, SearchFields, InActive)
					values(@ServiceId, @Tenant, 'WH', 'Warehousing', 'WH,Warehousing', 0)
			end

			if not exists (select Id from AdditionalServices where Tenant = @Tenant and Code = 'CC')
			begin
					EXECUTE usp_GetNextTableIdValue @ServiceId OUTPUT,'AdditionalService'
					insert into AdditionalServices(Id, Tenant, Code, Name, SearchFields, InActive)
					values(@ServiceId, @Tenant, 'CC', 'Customs Clearance', 'CC,Customs Clearance', 0)
			end

			if not exists (select Id from AdditionalServices where Tenant = @Tenant and Code = 'IN')
			begin
					EXECUTE usp_GetNextTableIdValue @ServiceId OUTPUT,'AdditionalService'
					insert into AdditionalServices(Id, Tenant, Code, Name, SearchFields, InActive)
					values(@ServiceId, @Tenant, 'IN', 'Insurance', 'IN,Insurance', 0)
			end

			if not exists (select Id from AdditionalServices where Tenant = @Tenant and Code = 'BB')
			begin
					EXECUTE usp_GetNextTableIdValue @ServiceId OUTPUT,'AdditionalService'
					insert into AdditionalServices(Id, Tenant, Code, Name, SearchFields, InActive)
					values(@ServiceId, @Tenant, 'BB', 'B2B', 'BB,B2B', 0)
			end					

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END