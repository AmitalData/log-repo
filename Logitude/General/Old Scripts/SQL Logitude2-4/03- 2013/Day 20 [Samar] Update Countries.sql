-- excute on main db

DECLARE @Tenant AS INT
BEGIN;

    declare @CountryId as varchar (15)
	DECLARE TenantCursor CURSOR READ_ONLY
	FOR	
	SELECT Id
	FROM Tenants	 
	OPEN TenantCursor FETCH NEXT FROM TenantCursor INTO @Tenant
	WHILE @@FETCH_STATUS = 0
		BEGIN

		update Ports
		set InActive = 1
		where CountryId = (select Id from Countries where Code = 'AN' and Tenant = @Tenant)

		update Countries
		set InActive = 1
		where Code = 'AN' and Tenant = @Tenant

		update Countries
		set EnglishName = 'Libya', LocalName = 'Libya'
		where Code = 'LY' and Tenant = @Tenant

		update Countries
		set EnglishName = 'State Of Palestine', LocalName = 'State Of Palestine'
		where Code = 'PS' and Tenant = @Tenant

		update Countries
		set EnglishName = 'United Kingdom', LocalName = 'United Kingdom'
		where Code = 'GB' and Tenant = @Tenant

		update Countries
		set EnglishName = 'United States', LocalName = 'United States'
		where Code = 'US' and Tenant = @Tenant

		update Countries
		set EnglishName = 'Republic Of Korea', LocalName = 'Republic Of Korea'
		where Code = 'KR' and Tenant = @Tenant

		update Countries
		set EnglishName = 'Democratic People''s Republic Of Korea', LocalName = 'Democratic Peoples Republic Of Korea'
		where Code = 'KP' and Tenant = @Tenant
		
		EXECUTE usp_GetNextTableIdValue @CountryId OUTPUT,'Country'
		insert into Countries (Id,Tenant, Code, EnglishName, LocalName, AddedManually, InActive, EC, GlobalZoneId) 
		values (@CountryId, @Tenant, 'BQ', 'Bonaire Sint Eustatius And Saba', 'Bonaire Sint Eustatius And Saba', 0, 0, 0, (select Id from GlobalZones where Code = 'EU' and Tenant = @Tenant))
		
		EXECUTE usp_GetNextTableIdValue @CountryId OUTPUT,'Country'
		insert into Countries (Id,Tenant, Code, EnglishName, LocalName, AddedManually, InActive, EC, GlobalZoneId) 
		values (@CountryId, @Tenant, 'CW', 'Curaçao', 'Curaçao', 0, 0, 0, (select Id from GlobalZones where Code = 'EU' and Tenant = @Tenant))
		
		EXECUTE usp_GetNextTableIdValue @CountryId OUTPUT,'Country'
		insert into Countries (Id,Tenant, Code, EnglishName, LocalName, AddedManually, InActive, EC, GlobalZoneId) 
		values (@CountryId, @Tenant, 'SX', 'Sint Maarten (Dutch Part)', 'Sint Maarten (Dutch Part)', 0, 0, 0, (select Id from GlobalZones where Code = 'EU' and Tenant = @Tenant))
		
		EXECUTE usp_GetNextTableIdValue @CountryId OUTPUT,'Country'
		insert into Countries (Id,Tenant, Code, EnglishName, LocalName, AddedManually, InActive, EC, GlobalZoneId) 
		values (@CountryId, @Tenant, 'SS', 'South Sudan', 'South Sudan', 0, 0, 0, (select Id from GlobalZones where Code = 'AF' and Tenant = @Tenant))

	FETCH NEXT FROM TenantCursor INTO @Tenant
	END
	CLOSE TenantCursor
	DEALLOCATE TenantCursor	
END
go


