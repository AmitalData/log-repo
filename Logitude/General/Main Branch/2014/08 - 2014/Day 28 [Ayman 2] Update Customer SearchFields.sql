
declare @Tenant as int
declare @CardId as varchar(15)
declare @ContactId as varchar(15)
declare @Code as varchar(15)
declare @EnglishName as varchar(60)
declare @LocalName as nvarchar(100)
declare @VatNumber as varchar(20)
declare @AccountingCard as varchar(25)
declare @CityName as nvarchar(25)
declare @CountryName as nvarchar(120)
declare @MySearchFields as nvarchar(1000)

declare @ContactMail as varchar(70)
declare @ContactName as varchar(60)
declare @Field1 as nvarchar(250)
declare @Field2 as nvarchar(250)
declare @Field3 as nvarchar(250)
declare @Field4 as nvarchar(250)
declare @Field5 as nvarchar(250)
declare @Field6 as nvarchar(250)
declare @Field7 as nvarchar(250)
declare @Field8 as nvarchar(250)
declare @Field9 as nvarchar(250)
declare @Field10 as nvarchar(250)

BEGIN 
		DECLARE CardsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Code, EnglishName, LocalName, VatNumber, AccountingCard, CityName, CountryName
		FROM Cards
		where PartnerTypeId = 'PO' or PartnerTypeId = 'CS' and searchfields is null
		OPEN CardsCursor FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant, @Code, @EnglishName, @LocalName, @VatNumber, @AccountingCard, @CityName, @CountryName
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @MySearchFields = ''

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
			
			-- Contacts
			BEGIN
				DECLARE CardContactsCursor CURSOR READ_ONLY
				FOR
				SELECT ContactId
				FROM CardContacts
				where CardId = @CardId AND Tenant = @Tenant
				OPEN CardContactsCursor FETCH NEXT FROM CardContactsCursor INTO @ContactId
				WHILE @@FETCH_STATUS = 0
				BEGIN
				
					select
					@ContactMail = Email,
					@ContactName = EnglishName
					from Contacts
					where Id = @ContactId AND Tenant = @Tenant
					
					if (@ContactMail is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @ContactMail
						else set @MySearchFields = @MySearchFields + ',' + @ContactMail
					end

					if (@ContactName is not null)
					begin
						if (@MySearchFields = '') set @MySearchFields = @ContactName
						else set @MySearchFields = @MySearchFields + ',' + @ContactName
					end

				FETCH NEXT FROM CardContactsCursor INTO @ContactId
				END
				CLOSE CardContactsCursor
				DEALLOCATE CardContactsCursor
			END
				
			-- CustomFields
			BEGIN

			select 
			@Field1 = Field1,
			@Field2 = Field2,
			@Field3 = Field3,
			@Field4 = Field4,
			@Field5 = Field5,
			@Field6 = Field6,
			@Field7 = Field7,
			@Field8 = Field8,
			@Field9 = Field9,
			@Field10 = Field10
			from Customers
			where Id = @CardId AND Tenant = @Tenant

				if (@Field1 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field1
					else set @MySearchFields = @MySearchFields + ',' + @Field1
				end
			
				if (@Field2 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field2
					else set @MySearchFields = @MySearchFields + ',' + @Field2
				end

				if (@Field3 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field3
					else set @MySearchFields = @MySearchFields + ',' + @Field3
				end

				if (@Field4 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field4
					else set @MySearchFields = @MySearchFields + ',' + @Field4
				end

				if (@Field5 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field5
					else set @MySearchFields = @MySearchFields + ',' + @Field5
				end

				if (@Field6 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field6
					else set @MySearchFields = @MySearchFields + ',' + @Field6
				end

				if (@Field7 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field7
					else set @MySearchFields = @MySearchFields + ',' + @Field7
				end

				if (@Field8 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field8
					else set @MySearchFields = @MySearchFields + ',' + @Field8
				end
			
				if (@Field9 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field9
					else set @MySearchFields = @MySearchFields + ',' + @Field9
				end

				if (@Field10 is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @Field10
					else set @MySearchFields = @MySearchFields + ',' + @Field10
				end
				
			END
			
			update Cards set SearchFields = @MySearchFields	where Id = @CardId and Tenant = @Tenant

		FETCH NEXT FROM CardsCursor INTO @CardId, @Tenant, @Code, @EnglishName, @LocalName, @VatNumber, @AccountingCard, @CityName, @CountryName
		END
		CLOSE CardsCursor
		DEALLOCATE CardsCursor
END

