
declare @Tenant as int
declare @SizeId as varchar(15)

BEGIN -- TenantsCursor
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant		
		WHILE @@FETCH_STATUS = 0
			BEGIN

			if not exists (select Id from CustomerSizes where Tenant = @Tenant and Rank = 1)
			begin
			
					EXECUTE usp_GetNextTableIdValue @SizeId OUTPUT,'CustomerSize'
					insert into CustomerSizes(Id, Tenant, Name, Rank, InActive, SearchFields)
					values(@SizeId, @Tenant, 'Small Business', 1, 0, '1,Small Business')
		    end

			if not exists (select Id from CustomerSizes where Tenant = @Tenant and Rank = 2)
			begin
					EXECUTE usp_GetNextTableIdValue @SizeId OUTPUT,'CustomerSize'
					insert into CustomerSizes(Id, Tenant, Name, Rank, InActive, SearchFields)
					values(@SizeId, @Tenant, 'Medium', 2, 0, '2,Medium')
			end

			if not exists (select Id from CustomerSizes where Tenant = @Tenant and Rank = 3)
			begin
					EXECUTE usp_GetNextTableIdValue @SizeId OUTPUT,'CustomerSize'
					insert into CustomerSizes(Id, Tenant, Name, Rank, InActive, SearchFields)
					values(@SizeId, @Tenant, 'Large', 3, 0, '3,Large')
			end

				FETCH NEXT FROM TenantsCursor INTO @Tenant	
			END
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END
