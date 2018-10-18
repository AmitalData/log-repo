
declare @Tenant as int
declare @OpportunityId as varchar(15)
declare @Subject as nvarchar(250)
declare @CustomerId as varchar(15)
declare @CustomerName as varchar(60)
declare @ContactId as varchar(15)
declare @ContactMail as varchar(70)
declare @ContactName as varchar(60)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE OpportunitiesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Subject, CustomerId, ContactId
		FROM Opportunities where searchfields is null
		OPEN OpportunitiesCursor FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @Subject, @CustomerId, @ContactId
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@Subject is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Subject
				else set @MySearchFields = @MySearchFields + ',' + @Subject	
			end
						
			if (@CustomerId is not null)
			begin

				set @CustomerName = (select EnglishName from Cards where Id = @CustomerId AND Tenant = @Tenant)

				if (@CustomerName is not null)
				begin
					if (@MySearchFields = '') set @MySearchFields = @CustomerName
					else set @MySearchFields = @MySearchFields + ',' + @CustomerName	
				end
			end

			if (@ContactId is not null)
			begin

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
			end

			update Opportunities set SearchFields = @MySearchFields where Id = @OpportunityId AND Tenant = @Tenant

		FETCH NEXT FROM OpportunitiesCursor INTO @OpportunityId, @Tenant, @Subject, @CustomerId, @ContactId

		END				
		CLOSE OpportunitiesCursor
		DEALLOCATE OpportunitiesCursor
END