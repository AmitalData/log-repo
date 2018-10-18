

declare @CardId as varchar(15)
declare @Tenant as int

declare @Code as varchar(15)
declare @EnglishName as varchar(60)
declare @LocalName as nvarchar(100)
declare @VatNumber as varchar(20)
declare @AccountingCard as varchar(25)
declare @CityName as nvarchar(25)
declare @CountryName as nvarchar(120)
declare @PartnerTypeId as char(2)
declare @Prefix as char(3)
declare @SearchText as nvarchar(150)

BEGIN -- CardsCursor
		DECLARE CardsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant,Code, EnglishName, LocalName, VatNumber, AccountingCard, CityName, CountryName, PartnerTypeId
		FROM Cards
		OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant,@Code, @EnglishName, @LocalName, @VatNumber, @AccountingCard, @CityName, @CountryName, @PartnerTypeId
		WHILE @@FETCH_STATUS = 0
			BEGIN

			set @SearchText = @Code + ',' + @EnglishName

			if(@LocalName is not null)
				set @SearchText += ',' + @LocalName
			
			if(@VatNumber is not null)
				set @SearchText += ',' + @VatNumber

			if(@AccountingCard is not null)
				set @SearchText += ',' + @AccountingCard

			if(@CityName is not null)
				set @SearchText += @CityName

			if(@CountryName is not null)			
				set @SearchText += ',' + @CountryName 

			if(@PartnerTypeId = 'AL')
			begin

				set @Prefix = null;
				set @Prefix = (select Prefix from Airlines where Id = @CardId and Tenant = @Tenant)

				if(@Prefix is not null)
				set @SearchText += ',' + @Prefix
			end

			update Cards
			set SearchFields = @SearchText
			where Id = @CardId and Tenant = @Tenant

				FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant,@Code, @EnglishName, @LocalName, @VatNumber, @AccountingCard, @CityName, @CountryName, @PartnerTypeId
			END
		CLOSE CardsCursor
		DEALLOCATE CardsCursor
END