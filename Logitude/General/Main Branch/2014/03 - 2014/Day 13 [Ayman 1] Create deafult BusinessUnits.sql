

declare @Tenant as int
declare @TenantString as varchar(50)

	DECLARE TenantsCursor CURSOR READ_ONLY
	FOR	
	SELECT Id	 
	FROM Tenants	
	OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant	
	WHILE @@FETCH_STATUS = 0
	BEGIN

		set @TenantString = convert(varchar,@Tenant)

		if not exists (select * from BusinessUnits where Id = @TenantString AND Tenant = @Tenant AND Name = 'Organization')
		begin
			insert into BusinessUnits(Id, Tenant, Name, InActive, SearchFields)
			values (@TenantString, @Tenant, 'Organization', 0 , 'Organization')
		end

	FETCH NEXT FROM TenantsCursor INTO @Tenant	
	END
	CLOSE TenantsCursor
	DEALLOCATE TenantsCursor