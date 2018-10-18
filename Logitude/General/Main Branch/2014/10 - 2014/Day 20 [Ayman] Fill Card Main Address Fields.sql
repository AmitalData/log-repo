



declare @Tenant as int
declare @CardId as varchar(15)
declare @City as nvarchar(25)
declare @CountryId as varchar(15)
declare @CountryCode as varchar(2)
declare @CountryName as varchar(120)
declare @MainAddressId as varchar(15)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant
		FROM Cards
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @CardId, @Tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @City = null
			set @CountryId = null
			set @CountryCode = null
			set @CountryName = null

			if exists (select * from Addresses where CardId = @CardId AND Tenant = @Tenant and AddressTypeId = 'M')
			begin
				set @MainAddressId = (select Max(Id) from Addresses where Tenant = @Tenant and CardId = @CardId and AddressTypeId = 'M')
				
				if(@MainAddressId is not null)
				begin
					select @City = City, @CountryId = CountryId from Addresses where Id = @MainAddressId and Tenant = @Tenant

					if (@CountryId is not null)
					begin
						select @CountryCode = Code, @CountryName = EnglishName from Countries where Id = @CountryId and Tenant = @Tenant
					end
				end
			end

			update Cards set
			CityName = @City,
			CountryId = @CountryId,
			CountryCode = @CountryCode,
			CountryName = @CountryName
			where Id = @CardId and Tenant = @Tenant

			FETCH NEXT FROM DataCursor INTO @CardId, @Tenant
			END
		CLOSE DataCursor
		DEALLOCATE DataCursor
END