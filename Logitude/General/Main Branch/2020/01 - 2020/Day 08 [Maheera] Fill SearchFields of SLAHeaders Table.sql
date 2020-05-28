declare @Tenant as int
declare @EntityId as varchar(15)
declare @Name as nvarchar(50)
declare @MySearchFields as nvarchar(1000)

BEGIN
		DECLARE SLAHeadersCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, Name
		FROM SLAHeaders
		OPEN SLAHeadersCursor FETCH NEXT FROM SLAHeadersCursor INTO @EntityId, @Tenant, @Name
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = @Name

		update SLAHeaders set SearchFields = @MySearchFields where Id = @EntityId AND Tenant = @Tenant

		FETCH NEXT FROM SLAHeadersCursor INTO @EntityId, @Tenant, @Name

		END				
		CLOSE SLAHeadersCursor
		DEALLOCATE SLAHeadersCursor
END