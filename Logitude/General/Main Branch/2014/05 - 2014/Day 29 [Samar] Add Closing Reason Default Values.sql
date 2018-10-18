
declare @Tenant as int
declare @ReasonId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from OpportunityClosingReasons where Tenant = @Tenant and Code = 'CA')
			begin
			
					EXECUTE usp_GetNextTableIdValue @ReasonId OUTPUT,'OpportunityClosingReason'
					insert into OpportunityClosingReasons(Id, Tenant, Code, Name, LocalName, SearchFields, IsClosedLost, AddedManually)
					values(@ReasonId, @Tenant, 'CA', 'Cancelled','Cancelled', 'Cancelled,Cancelled', 0, 0)
		    end

			if not exists (select Id from OpportunityClosingReasons where Tenant = @Tenant and Code = 'LC')
			begin
					EXECUTE usp_GetNextTableIdValue @ReasonId OUTPUT,'OpportunityClosingReason'
					insert into OpportunityClosingReasons(Id, Tenant, Code, Name, LocalName, SearchFields, IsClosedLost, AddedManually)
					values(@ReasonId, @Tenant, 'LC', 'Lost To Competition','Lost To Competition', 'Lost To Competition,Lost To Competition', 1, 0)
			end

			if not exists (select Id from OpportunityClosingReasons where Tenant = @Tenant and Code = 'LO')
			begin
					EXECUTE usp_GetNextTableIdValue @ReasonId OUTPUT,'OpportunityClosingReason'
					insert into OpportunityClosingReasons(Id, Tenant, Code, Name, LocalName, SearchFields, IsClosedLost, AddedManually)
					values(@ReasonId, @Tenant, 'LO', 'Lost', 'Lost', 'Lost,Lost', 1, 0)
			end

			if not exists (select Id from OpportunityClosingReasons where Tenant = @Tenant and Code = 'WN')
			begin
					EXECUTE usp_GetNextTableIdValue @ReasonId OUTPUT,'OpportunityClosingReason'
					insert into OpportunityClosingReasons(Id, Tenant, Code, Name, LocalName, SearchFields, IsClosedLost, AddedManually)
					values(@ReasonId, @Tenant, 'WN', 'Won', 'Won', 'Won, Won', 0, 0)
			end				

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END