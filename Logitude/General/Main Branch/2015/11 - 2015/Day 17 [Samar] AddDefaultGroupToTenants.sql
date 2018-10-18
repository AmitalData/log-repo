
declare @UserId as varchar(15)
declare @Tenant as int
declare @EmployeeGroupId as varchar(15)

BEGIN 
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @UserId = (select Id from Contacts where Email like '%system@tenant%' and Tenant = @Tenant)

			if not exists (select Id from EmployeeGroups where Tenant = @Tenant and Name = 'Unassigned Tickets')
			begin			
					EXECUTE usp_GetNextTableIdValue @EmployeeGroupId OUTPUT,'EmployeeGroup'
					insert into EmployeeGroups(Id, Tenant, CreateDate, CreatedByUserId, UpdateDate, UpdatedByUserId, SearchFields, Name, [Description], Inactive, EscalationUserId)
					values(@EmployeeGroupId, @Tenant, GETDATE() , @UserId, GETDATE(), @UserId, 'Unassigned Tickets', 'Unassigned Tickets', null, 0, null)
		    end

			update TicketClassifications set EmployeeGroupId = @EmployeeGroupId where Name = 'General' and Tenant = @Tenant

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END

