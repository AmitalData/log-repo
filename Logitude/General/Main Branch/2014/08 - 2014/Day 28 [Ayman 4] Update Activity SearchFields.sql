
declare @Tenant as int
declare @EntityId as varchar(15)
declare @Subject as nvarchar(255)
declare @Description as nvarchar(max)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE ActivitiesCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Subject, Description
		FROM Activities
		OPEN ActivitiesCursor FETCH NEXT FROM ActivitiesCursor INTO @EntityId, @Tenant, @Subject, @Description
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
			
			update Activities set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM ActivitiesCursor INTO @EntityId, @Tenant, @Subject, @Description

		END				
		CLOSE ActivitiesCursor
		DEALLOCATE ActivitiesCursor
END