
declare @Tenant as int
declare @AddressId as varchar(15)
declare @CountryId as varchar(15)
declare @CountryCode as varchar(5)

BEGIN
		DECLARE TenantsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, AddressId
		FROM Tenants
		OPEN TenantsCursor FETCH NEXT FROM TenantsCursor INTO @Tenant, @AddressId
		WHILE @@FETCH_STATUS = 0
		BEGIN
			set @CountryId = null
			set @CountryCode = null

			if (@AddressId is not null)
			begin
				set @CountryId = (select CountryId from Addresses where Id = @AddressId and Tenant = @Tenant)
			end

			if (@CountryId is not null)
			begin
				set @CountryCode = (select Code from Countries where Id = @CountryId and Tenant = @Tenant)
			end

			if (@CountryCode = 'MA')
			begin
				if exists (select Tenant from CustomsInterfaceSettings where Tenant = @Tenant)
				begin 		           
					update CustomsInterfaceSettings set ActivateCustomsManagementInShipments = 1 where Tenant = @Tenant                     
				end 
			end

		FETCH NEXT FROM TenantsCursor INTO @Tenant, @AddressId

		END				
		CLOSE TenantsCursor
		DEALLOCATE TenantsCursor
END