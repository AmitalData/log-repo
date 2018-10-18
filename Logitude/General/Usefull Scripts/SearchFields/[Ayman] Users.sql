

declare @Tenant as int
declare @EntityId as varchar(15)

declare @Code as varchar(20)
declare @PersonalId as varchar(20)
declare @Email as varchar(70)
declare @EnglishName as varchar(60)
declare @LocalName as nvarchar(100)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE DataCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Code, PersonalId
		FROM Users
		WHERE SearchFields is null
		OPEN DataCursor FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant, @Code, @PersonalId
		WHILE @@FETCH_STATUS = 0
		BEGIN

			set @Email = null
			set @EnglishName = null
			set @LocalName = null
			set @MySearchFields = ''

			if exists (select * from Contacts where Id = @EntityId AND Tenant = @Tenant)
			begin
				select
				@Email = Email,
				@EnglishName = EnglishName,
				@LocalName = LocalName
				from Contacts
				where Id = @EntityId AND Tenant = @Tenant
			end

			if (@Code is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Code
				else set @MySearchFields = @MySearchFields + ',' + @Code	
			end

			if (@PersonalId is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @PersonalId
				else set @MySearchFields = @MySearchFields + ',' + @PersonalId	
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

			if (@Email is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Email
				else set @MySearchFields = @MySearchFields + ',' + @Email	
			end

			update Users set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM DataCursor INTO @EntityId, @Tenant,  @Code, @PersonalId
		END				
		CLOSE DataCursor
		DEALLOCATE DataCursor
END