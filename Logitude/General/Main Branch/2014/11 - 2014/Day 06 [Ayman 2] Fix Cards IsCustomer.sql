
declare @Tenant as int
declare @EntityId as varchar(15)
declare @IsCustomer as bit

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, IsCustomer
		FROM Customers
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @IsCustomer
		WHILE @@FETCH_STATUS = 0
			BEGIN

				update Cards set IsCustomer = @IsCustomer
				where Tenant = @Tenant AND Id = @EntityId

			FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @IsCustomer	
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END