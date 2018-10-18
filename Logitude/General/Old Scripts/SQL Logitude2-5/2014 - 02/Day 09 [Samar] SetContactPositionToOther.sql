
update Contacts set ContactPositionId = null
go

delete from ContactPositions
go

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

				update Contacts 
				set ContactPositionId = (select Id from ContactPositions where Tenant = @Tenant and Code = 'OTH')
				where Tenant = @Tenant

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END