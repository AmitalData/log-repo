
select * from Countries where HasStates = 1

DECLARE @CountryId varchar(15)
DECLARE @Tenant int

		DECLARE CountriesCursor CURSOR READ_ONLY
		FOR
		SELECT Id,Tenant
		FROM Countries
		OPEN CountriesCursor FETCH NEXT FROM CountriesCursor INTO @CountryId,@Tenant
		WHILE @@FETCH_STATUS = 0
			BEGIN

				if exists (select * from States where CountryId = @CountryId AND Tenant = @Tenant)				
				update Countries set HasStates = 1 where Id = @CountryId AND Tenant = @Tenant
				
				else
				update Countries set HasStates = 0 where Id = @CountryId AND Tenant = @Tenant

			FETCH NEXT FROM CountriesCursor INTO @CountryId,@Tenant	
			END
		CLOSE CountriesCursor
		DEALLOCATE CountriesCursor