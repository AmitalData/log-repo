declare @Tenant as int
declare @PortId as varchar(15)
declare @CombinedCode as nvarchar(30)
declare @MySearchFields as nvarchar(1000)
declare @SearchFields as nvarchar(1000)


BEGIN
		DECLARE PortCursor CURSOR READ_ONLY
		FOR
		SELECT Id, Tenant, CombinedCode, SearchFields
		FROM Ports 
		OPEN PortCursor FETCH NEXT FROM PortCursor INTO @PortId, @Tenant, @CombinedCode, @SearchFields
		WHILE @@FETCH_STATUS = 0
		BEGIN

		set @MySearchFields = @SearchFields

			if (@CombinedCode is not null)
			begin
				 set @MySearchFields = @MySearchFields + ',' + @CombinedCode	
			end

			update Ports set SearchFields = @MySearchFields where Id = @PortId AND Tenant = @Tenant

		FETCH NEXT FROM PortCursor INTO @PortId, @Tenant, @CombinedCode, @SearchFields

		END				
		CLOSE PortCursor
		DEALLOCATE PortCursor
END