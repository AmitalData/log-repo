
declare @Tenant as int
declare @EntityId as varchar(15)
declare @EnglishName as varchar(60)
declare @LocalName as nvarchar(100)
declare @Email as varchar(70)
declare @BusinessPhone as varchar(25)
declare @Mobile as varchar(25)
declare @Fax as varchar(25)
declare @MySearchFields as nvarchar(1000)
declare @CardId as varchar(15)
declare @Company as varchar(1000)
declare @CardEnglishName as varchar(70)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, EnglishName, LocalName, Email, BusinessPhone, Mobile, Fax
		FROM Contacts
		--WHERE SearchFields is null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @EnglishName, @LocalName, @Email, @BusinessPhone, @Mobile, @Fax
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @Company = null
		set @MySearchFields = ''

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

			if (@Email is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Email
				else set @MySearchFields = @MySearchFields + ',' + @Email	
			end

			if (@BusinessPhone is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @BusinessPhone
				else set @MySearchFields = @MySearchFields + ',' + @BusinessPhone	
			end

			if (@Mobile is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Mobile
				else set @MySearchFields = @MySearchFields + ',' + @Mobile	
			end

			if (@Fax is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Fax
				else set @MySearchFields = @MySearchFields + ',' + @Fax	
			end


			-- CardContacts
			DECLARE CardContactsCursor CURSOR READ_ONLY
			FOR
			SELECT CardId
			FROM CardContacts
			where ContactId = @EntityId and Tenant = @Tenant
			OPEN CardContactsCursor FETCH NEXT FROM CardContactsCursor INTO @CardId
			WHILE @@FETCH_STATUS = 0
			BEGIN

				set @CardEnglishName = (select EnglishName from Cards where Id = @CardId and Tenant = @Tenant)

				if (@CardEnglishName is not null)
				begin
					if (@Company = '' OR @Company is null) set @Company = @CardEnglishName
					else set @Company = @Company + ',' + @CardEnglishName	
				end

			FETCH NEXT FROM CardContactsCursor INTO @CardId
			END				
			CLOSE CardContactsCursor
			DEALLOCATE CardContactsCursor

				
			if (@Company is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Company
				else set @MySearchFields = @MySearchFields + ',' + @Company	
			end


			update Contacts
			set
			CompanyName = @Company,
			SearchFields = @MySearchFields
			where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @EnglishName, @LocalName, @Email, @BusinessPhone, @Mobile, @Fax
		END				
		CLOSE DataCursor
		DEALLOCATE DataCursor
END

