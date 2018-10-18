declare @Tenant as int
declare @EntityId as varchar(15)
declare @ProjectNumber as varchar(12)
declare @Name as nvarchar(60)
declare @CustomerId as varchar(15)
declare @CustomerName as varchar(60)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE TMProjectsCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, ProjectNumber, Name, CustomerId
		FROM TMProjects
		OPEN TMProjectsCursor FETCH NEXT FROM TMProjectsCursor INTO @EntityId, @Tenant,@ProjectNumber, @Name, @CustomerId
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = ''

			if (@ProjectNumber is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @ProjectNumber
				else set @MySearchFields = @MySearchFields + ',' + @ProjectNumber	
			end
			
			if (@Name is not null)
			begin
				if (@MySearchFields = '') set @MySearchFields = @Name
				else set @MySearchFields = @MySearchFields + ',' + @Name	
			end

			if (@CustomerId is not null)
			begin
				set @CustomerName = (select EnglishName from Cards where Tenant = @Tenant and Id = @CustomerId)

				if (@MySearchFields = '') set @MySearchFields = @CustomerName
				else set @MySearchFields = @MySearchFields + ',' + @CustomerName	
			end

			update TMProjects set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM TMProjectsCursor INTO @EntityId, @Tenant,@ProjectNumber, @Name, @CustomerId

		END				
		CLOSE TMProjectsCursor
		DEALLOCATE TMProjectsCursor
END

select * from TMProjects