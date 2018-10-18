
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

			set @PositionId = (select Id from ContactPositions where Tenant = @Tenant and Code = 'WRM')

			update ContactPositions
			set Name = 'Warehouse Manager', SearchFields = 'Warehouse Manager'
			where Id = @PositionId and Tenant = @Tenant			

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END