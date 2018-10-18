
declare @Tenant as int
declare @EntityId as varchar(15)
declare @Prefix as varchar(3)
declare @ICAO as varchar(3)
declare @Code as varchar(15)
declare @EnglishName as varchar(70)
declare @LocalName as varchar(100)
declare @VatNumber as varchar(20)
declare @AccountingCard as varchar(25)
declare @CityName as varchar(25)
declare @CountryName as varchar(120)
declare @MySearchFields as nvarchar(max)

BEGIN
		DECLARE AirlinesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Prefix, ICAO
		FROM Airlines
		OPEN AirlinesCursor FETCH NEXT FROM AirlinesCursor INTO @EntityId, @Tenant, @Prefix, @ICAO
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@Prefix is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Prefix
				else set @MySearchFields = @MySearchFields + ',' + @Prefix	
			end			
						
			if (@ICAO is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ICAO
				else set @MySearchFields = @MySearchFields + ',' + @ICAO	
			end			

			set @Code = (select Code from Cards where Tenant = @Tenant and Id = @EntityId)
			set @EnglishName = (select EnglishName from Cards where Tenant = @Tenant and Id = @EntityId)
			set @LocalName = (select LocalName from Cards where Tenant = @Tenant and Id = @EntityId)
			set @VatNumber = (select VatNumber from Cards where Tenant = @Tenant and Id = @EntityId)
			set @AccountingCard = (select AccountingCard from Cards where Tenant = @Tenant and Id = @EntityId)
			set @CityName = (select CityName from Cards where Tenant = @Tenant and Id = @EntityId)
			set @CountryName = (select CountryName from Cards where Tenant = @Tenant and Id = @EntityId)

			if (@Code is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Code
				else set @MySearchFields = @MySearchFields + ',' + @Code	
			end	

			if (@EnglishName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @EnglishName
				else set @MySearchFields = @MySearchFields + ',' + @EnglishName	
			end	

			if (@LocalName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @LocalName
				else set @MySearchFields = @MySearchFields + ',' + @LocalName	
			end	

			if (@VatNumber is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @VatNumber
				else set @MySearchFields = @MySearchFields + ',' + @VatNumber	
			end	

			if (@AccountingCard is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @AccountingCard
				else set @MySearchFields = @MySearchFields + ',' + @AccountingCard	
			end	

			if (@CityName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @CityName
				else set @MySearchFields = @MySearchFields + ',' + @CityName	
			end	

			if (@CountryName is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @CountryName
				else set @MySearchFields = @MySearchFields + ',' + @CountryName	
			end	
			
			update Cards set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM AirlinesCursor INTO @EntityId, @Tenant, @Prefix, @ICAO

		END				
		CLOSE AirlinesCursor
		DEALLOCATE AirlinesCursor
END