	declare @Tenant as int
	declare @PortId as varchar(15)
	declare @Code as varchar(15)
	declare @CountryId as varchar(15)
	declare @CombinedCode as varchar(10)
	declare @CountryCode as varchar(5)

	DECLARE PortsCursor CURSOR READ_ONLY
	FOR
	SELECT Id, Tenant, Code, CountryId
	FROM Ports
	OPEN PortsCursor FETCH NEXT FROM PortsCursor INTO @PortId, @Tenant, @Code, @CountryId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT @CountryCode = Code FROM Countries where Id = @CountryId

		set @CombinedCode = @CountryCode + @Code

		update Ports set
		CombinedCode = @CombinedCode
		where Id = @PortId and CombinedCode is null

	FETCH NEXT FROM PortsCursor INTO @PortId, @Tenant, @Code, @CountryId
	END
	CLOSE PortsCursor
	DEALLOCATE PortsCursor