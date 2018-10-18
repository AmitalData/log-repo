


declare @Tenant as int
declare @EntityId as varchar(15)
declare @SalesmanUserId as varchar(15)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, SalesmanUserId
		FROM Customers
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @SalesmanUserId
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update Cards
				set SalesmanUserId = @SalesmanUserId
				where Id = @EntityId AND Tenant = @Tenant

			FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @SalesmanUserId	
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END