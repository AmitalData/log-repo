

declare @Tenant as int
declare @PositionId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN


			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'OTH')
			begin
			
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'OTH', 'Other', 0, 'Other')
		    end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'GMN')
			begin TransportDocumentNumber
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'GMN', 'General Manager', 0, 'General Manager')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'FMN')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'FMN', 'Financial Manager', 0, 'Financial Manager')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'BUY')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'BUY', 'Buyer', 0, 'Buyer')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'SAL')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'SAL', 'Salesman', 0, 'Salesman')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'LMN')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'LMN', 'Logistic Manager', 0, 'Logistic Manager')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'WRM')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'WRM', 'Warehouse Man', 0, 'Warehouse Man')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'IMN')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'IMN', 'Import Manager', 0, 'Import Manager')
			end

			if not exists (select Id from ContactPositions where Tenant = @Tenant and Code = 'EXM')
			begin
					EXECUTE usp_GetNextTableIdValue @PositionId OUTPUT,'ContactPosition'
					insert into ContactPositions(Id, Tenant, Code, Name, InActive, SearchFields)
					values(@PositionId, @Tenant, 'EXM', 'Export Manager', 0, 'Export Manager')		
			end

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END