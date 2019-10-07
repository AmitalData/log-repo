
	declare @Tenant as int
	declare @TariffId as varchar(15)
	declare @CurrencyId as varchar(15)

	DECLARE TariffsCursor CURSOR READ_ONLY
	FOR
	SELECT Tenant, Id, CurrencyId
	FROM Tariffs	
	where TypeCode = 'ASC'
	OPEN TariffsCursor FETCH NEXT FROM TariffsCursor INTO @Tenant, @TariffId, @CurrencyId
	WHILE @@FETCH_STATUS = 0
	BEGIN

		update TariffLines
		set CurrencyId = @CurrencyId
		where TariffId = @TariffId


	FETCH NEXT FROM TariffsCursor INTO @Tenant, @TariffId, @CurrencyId
	END
	CLOSE TariffsCursor
	DEALLOCATE TariffsCursor