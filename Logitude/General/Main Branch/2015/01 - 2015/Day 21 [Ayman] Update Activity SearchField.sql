
declare @Tenant as int
declare @EntityId as varchar(15)
declare @Subject as nvarchar(255)
declare @CustomerId as varchar(15)
declare @CustomerName as varchar(60)
declare @Description as nvarchar(max)
declare @MySearchFields as nvarchar(max)

BEGIN
		DECLARE ActivitiesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Subject, Description, CustomerId
		FROM Activities
		OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @EntityId, @Tenant, @Subject, @Description, @CustomerId
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@Subject is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Subject
				else set @MySearchFields = @MySearchFields + ',' + @Subject	
			end
						
			if (@Description is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Description
				else set @MySearchFields = @MySearchFields + ',' + @Description	
			end
			
			if (@CustomerId is not null)
			begin
				set @CustomerName = (select EnglishName from Cards where Tenant = @Tenant and Id = @CustomerId)

				if (@MySearchFields = '') set @MySearchFields = @CustomerName
				else set @MySearchFields = @MySearchFields + ',' + @CustomerName	
			end

			update Activities set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM ActivitiesCursor INTO @EntityId, @Tenant, @Subject, @Description, @CustomerId

		END				
		CLOSE ActivitiesCursor
		DEALLOCATE ActivitiesCursor
END